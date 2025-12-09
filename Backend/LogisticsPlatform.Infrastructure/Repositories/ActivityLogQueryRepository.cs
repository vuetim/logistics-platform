using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class ActivityLogQueryRepository : IActivityLogQueryRepository
    {
        private readonly AppDbContext _context;

        public ActivityLogQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogDto>> GetByEntityAsync(
            string entityType,
            Guid entityId)
        {
            return await (
                from log in _context.ActivityLogs
                join user in _context.Users
                    on log.PerformedByUserId equals user.Id
                where log.EntityType == entityType
                   && log.EntityId == entityId
                orderby log.CreatedAt descending
                select new ActivityLogDto
                {
                    Action = log.Summary,
                    Field = null,             
                    OldValue = null,
                    NewValue = null,
                    Details = log.Details,       
                    PerformedBy = user.FullName,
                    CreatedAt = log.CreatedAt
                }
            ).ToListAsync();
        }
    }
}
