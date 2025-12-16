namespace LogisticsPlatform.Application.Interfaces.Services;

public interface IAuthAuditService
{
    Task LogAsync(
        Guid? userId,
        string eventName,
        string? ipAddress,
        string? userAgent);
}
