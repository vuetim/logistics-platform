using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Domain.Entities;

public class ActivityLogService : IActivityLogService
{
    private readonly IActivityLogRepository _repo;

    public ActivityLogService(IActivityLogRepository repo)
    {
        _repo = repo;
    }

    public async Task LogAsync(ActivityLogEntry entry )
    {
        var log = new ActivityLog  
        {
            EntityType = entry.EntityType,
            EntityId = entry.EntityId,
            ActivityType = entry.ActivityType,
            Summary = entry.Summary,
            Details = entry.Details,
            PerformedByUserId = entry.PerformedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(log);
        await _repo.SaveChangesAsync();
    }
}
