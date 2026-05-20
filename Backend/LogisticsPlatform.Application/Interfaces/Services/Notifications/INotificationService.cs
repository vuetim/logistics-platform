using LogisticsPlatform.Application.DTOs.Notifications;

namespace LogisticsPlatform.Application.Interfaces.Services.Notifications;

public interface INotificationService
{
    Task NotifyLoadNoteAddedAsync(Guid loadId, Guid actorUserId, string noteText);
    Task NotifyLoadStopEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task NotifyCarrierTenderEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task NotifyLoadExceptionEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task NotifyLoadDocumentEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task NotifyInvoiceEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task NotifySettlementEventAsync(Guid loadId, Guid actorUserId, string summary);
    Task<List<NotificationDto>> GetRecentAsync(Guid userId, int take = 20);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkReadAsync(Guid notificationId, Guid userId);
    Task MarkAllReadAsync(Guid userId);
    Task DeleteExpiredAsync(DateTime utcNow);
}
