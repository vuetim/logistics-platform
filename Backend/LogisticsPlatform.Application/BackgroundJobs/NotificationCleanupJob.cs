using LogisticsPlatform.Application.Interfaces.Services.Notifications;

namespace LogisticsPlatform.Application.BackgroundJobs;

public class NotificationCleanupJob
{
    private readonly INotificationService _notifications;

    public NotificationCleanupJob(INotificationService notifications)
    {
        _notifications = notifications;
    }

    public Task ExecuteAsync()
        => _notifications.DeleteExpiredAsync(DateTime.UtcNow);
}
