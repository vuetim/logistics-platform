using LogisticsPlatform.Application.DTOs.Notifications;
using LogisticsPlatform.Application.Interfaces.Repositories.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Domain.Entities.Security;

namespace LogisticsPlatform.Application.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notifications;
    private readonly INotificationRecipientRepository _recipients;

    public NotificationService(
        INotificationRepository notifications,
        INotificationRecipientRepository recipients)
    {
        _notifications = notifications;
        _recipients = recipients;
    }

    public Task NotifyLoadNoteAddedAsync(Guid loadId, Guid actorUserId, string noteText)
    {
        return CreateForInternalUsersAsync(
            actorUserId,
            "Load note added",
            Trim(noteText, 180),
            "LoadNoteAdded",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=notes",
            TimeSpan.FromHours(24));
    }

    public Task NotifyLoadStopEventAsync(Guid loadId, Guid actorUserId, string summary)
    {
        return CreateForInternalUsersAsync(
            actorUserId,
            "Load event updated",
            Trim(summary, 180),
            "LoadStopEvent",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=stops",
            TimeSpan.FromHours(24));
    }

    public Task NotifyCarrierTenderEventAsync(Guid loadId, Guid actorUserId, string summary)
        => CreateForInternalUsersAsync(
            actorUserId,
            "Carrier tender updated",
            Trim(summary, 180),
            "CarrierTender",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=tenders",
            TimeSpan.FromHours(24));

    public Task NotifyLoadExceptionEventAsync(Guid loadId, Guid actorUserId, string summary)
        => CreateForInternalUsersAsync(
            actorUserId,
            "Load exception updated",
            Trim(summary, 180),
            "LoadException",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=exceptions",
            TimeSpan.FromHours(24));

    public Task NotifyLoadDocumentEventAsync(Guid loadId, Guid actorUserId, string summary)
        => CreateForInternalUsersAsync(
            actorUserId,
            "Load document updated",
            Trim(summary, 180),
            "LoadDocument",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=documents",
            TimeSpan.FromHours(24));

    public Task NotifyInvoiceEventAsync(Guid loadId, Guid actorUserId, string summary)
        => CreateForInternalUsersAsync(
            actorUserId,
            "Invoice updated",
            Trim(summary, 180),
            "Invoice",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=billing",
            TimeSpan.FromHours(24));

    public Task NotifySettlementEventAsync(Guid loadId, Guid actorUserId, string summary)
        => CreateForInternalUsersAsync(
            actorUserId,
            "Settlement updated",
            Trim(summary, 180),
            "Settlement",
            "Load",
            loadId,
            $"/loads/{loadId}?tab=billing",
            TimeSpan.FromHours(24));

    public async Task<List<NotificationDto>> GetRecentAsync(Guid userId, int take = 20)
    {
        take = Math.Clamp(take, 1, 50);
        var notifications = await _notifications.GetRecentAsync(userId, DateTime.UtcNow, take);

        return notifications.Select(x => new NotificationDto
        {
            Id = x.Id,
            Title = x.Title,
            Message = x.Message,
            Type = x.Type,
            ActorName = x.ActorUser.FullName,
            TargetType = x.TargetType,
            TargetId = x.TargetId,
            Route = x.Route,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt,
            ExpiresAt = x.ExpiresAt
        }).ToList();
    }

    public Task<int> GetUnreadCountAsync(Guid userId)
        => _notifications.CountUnreadAsync(userId, DateTime.UtcNow);

    public async Task MarkReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _notifications.GetByIdForUserAsync(notificationId, userId);
        if (notification == null || notification.IsRead) return;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;
        await _notifications.SaveChangesAsync();
    }

    public Task MarkAllReadAsync(Guid userId)
        => _notifications.MarkAllReadAsync(userId, DateTime.UtcNow);

    public Task DeleteExpiredAsync(DateTime utcNow)
        => _notifications.DeleteExpiredAsync(utcNow);

    private async Task CreateForInternalUsersAsync(
        Guid actorUserId,
        string title,
        string message,
        string type,
        string targetType,
        Guid targetId,
        string route,
        TimeSpan ttl)
    {
        var recipientIds = await _recipients.GetInternalRecipientIdsAsync(actorUserId);
        if (recipientIds.Count == 0) return;

        var now = DateTime.UtcNow;
        var notifications = recipientIds.Select(userId => new UserNotification
        {
            UserId = userId,
            ActorUserId = actorUserId,
            Title = title,
            Message = message,
            Type = type,
            TargetType = targetType,
            TargetId = targetId,
            Route = route,
            CreatedAt = now,
            ExpiresAt = now.Add(ttl)
        });

        await _notifications.AddRangeAsync(notifications);
        await _notifications.SaveChangesAsync();
    }

    private static string Trim(string value, int maxLength)
    {
        value = value.Trim();
        return value.Length <= maxLength ? value : value[..maxLength].TrimEnd() + "...";
    }
}
