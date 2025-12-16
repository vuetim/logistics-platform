using LogisticsPlatform.Application.DTOs.ActivityLog;

namespace LogisticsPlatform.Application.Interfaces.Services.ActivityLog
{
    public interface IActivityLogService
    {
        Task LogAsync(ActivityLogEntry entry);
    }
}
