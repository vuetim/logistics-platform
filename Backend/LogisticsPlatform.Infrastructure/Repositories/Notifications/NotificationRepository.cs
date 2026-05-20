using LogisticsPlatform.Application.Interfaces.Repositories.Notifications;
using LogisticsPlatform.Domain.Entities.Security;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Notifications;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _db;

    public NotificationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddRangeAsync(IEnumerable<UserNotification> notifications)
    {
        await _db.UserNotifications.AddRangeAsync(notifications);
    }

    public Task<List<UserNotification>> GetRecentAsync(Guid userId, DateTime utcNow, int take)
        => _db.UserNotifications
            .AsNoTracking()
            .Include(x => x.ActorUser)
            .Where(x => x.UserId == userId && x.ExpiresAt > utcNow)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync();

    public Task<int> CountUnreadAsync(Guid userId, DateTime utcNow)
        => _db.UserNotifications
            .CountAsync(x => x.UserId == userId && !x.IsRead && x.ExpiresAt > utcNow);

    public Task<UserNotification?> GetByIdForUserAsync(Guid notificationId, Guid userId)
        => _db.UserNotifications
            .FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

    public Task MarkAllReadAsync(Guid userId, DateTime utcNow)
        => _db.UserNotifications
            .Where(x => x.UserId == userId && !x.IsRead && x.ExpiresAt > utcNow)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsRead, true)
                .SetProperty(x => x.ReadAt, utcNow)
                .SetProperty(x => x.UpdatedAt, utcNow));

    public Task DeleteExpiredAsync(DateTime utcNow)
        => _db.UserNotifications
            .Where(x => x.ExpiresAt <= utcNow)
            .ExecuteDeleteAsync();

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
