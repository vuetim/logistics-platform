using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities;

public class ActivityLog
{
    public Guid Id { get; set; }

    public string EntityType { get; set; } = default!;
    public Guid EntityId { get; set; }

    public ActivityType ActivityType { get; set; }

    public string Summary { get; set; } = default!;
    public string? Details { get; set; }

    public Guid PerformedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
