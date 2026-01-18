using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Services;
using System.Text.Json;

public class AuthAuditService : IAuthAuditService
{
    private readonly IAuthAuditLogRepository _repo;

    public AuthAuditService(IAuthAuditLogRepository repo)
    {
        _repo = repo;
    }

    public async Task LogAsync(
        Guid? actorUserId,
        string eventName,
        Guid? targetUserId = null,
        object? metadata = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        var log = new AuthAuditLog
        {
            ActorUserId = actorUserId,
            TargetUserId = targetUserId,
            Event = eventName,
            Metadata = metadata != null
                ? JsonSerializer.Serialize(metadata)
                : null,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _repo.AddAsync(log);
        await _repo.SaveChangesAsync();
    }
}
