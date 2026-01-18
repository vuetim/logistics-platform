using LogisticsPlatform.Domain.Common;

public class AuthAuditLog : BaseEntity
{
    public Guid? ActorUserId { get; set; }
    public Guid? TargetUserId { get; set; }

    public string Event { get; set; } = null!;

    public string? Metadata { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
