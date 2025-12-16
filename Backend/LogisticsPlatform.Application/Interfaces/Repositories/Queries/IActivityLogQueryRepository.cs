using LogisticsPlatform.Application.DTOs.ActivityLog;

public interface IActivityLogQueryRepository
{
    Task<List<ActivityLogDto>> GetByEntityAsync(
        string entityType,
        Guid entityId);
}
