namespace LogisticsPlatform.Application.Interfaces.Repositories.Notifications;

public interface INotificationRecipientRepository
{
    Task<List<Guid>> GetInternalRecipientIdsAsync(Guid actorUserId);
}
