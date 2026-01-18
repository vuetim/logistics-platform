namespace LogisticsPlatform.Application.Interfaces.Services;

public interface IAuthAuditService
{
    Task LogAsync(
        Guid? actorUserId,
        string eventName,
        Guid? targetUserId = null,
        object? metadata = null,
        string? ipAddress = null,
        string? userAgent = null);
}
