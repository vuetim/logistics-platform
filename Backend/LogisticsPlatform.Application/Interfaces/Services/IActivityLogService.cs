using LogisticsPlatform.Application.DTOs.ActivityLog;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface IActivityLogService
    {
        Task LogAsync(ActivityLogEntry entry);
    }
}
