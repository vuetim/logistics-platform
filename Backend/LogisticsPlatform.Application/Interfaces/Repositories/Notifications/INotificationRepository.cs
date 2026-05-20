using LogisticsPlatform.Domain.Entities.Security;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Notifications;

public interface INotificationRepository
{
    Task AddRangeAsync(IEnumerable<UserNotification> notifications);
    Task<List<UserNotification>> GetRecentAsync(Guid userId, DateTime utcNow, int take);
    Task<int> CountUnreadAsync(Guid userId, DateTime utcNow);
    Task<UserNotification?> GetByIdForUserAsync(Guid notificationId, Guid userId);
    Task MarkAllReadAsync(Guid userId, DateTime utcNow);
    Task DeleteExpiredAsync(DateTime utcNow);
    Task SaveChangesAsync();
}
