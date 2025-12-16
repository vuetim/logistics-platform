using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities.Security;

namespace LogisticsPlatform.Infrastructure.Services;

public class AuthAuditService : IAuthAuditService
{
    private readonly IAuthAuditLogRepository _repo;

    public AuthAuditService(IAuthAuditLogRepository repo)
    {
        _repo = repo;
    }

    public async Task LogAsync(
        Guid? userId,
        string eventName,
        string? ipAddress,
        string? userAgent)
    {
        var log = new AuthAuditLog
        {
            UserId = userId,
            Event = eventName,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _repo.AddAsync(log);
        await _repo.SaveChangesAsync();
    }
}
