using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities.Security;

public class AuthAuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Event { get; set; } = null!;

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
