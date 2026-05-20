using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities.Security;

public class UserNotification : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ActorUserId { get; set; }
    public User ActorUser { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public Guid TargetId { get; set; }
    public string Route { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
