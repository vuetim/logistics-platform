using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories;

public interface IActivityLogRepository
{
    Task AddAsync(ActivityLog log);
    Task SaveChangesAsync();
}
