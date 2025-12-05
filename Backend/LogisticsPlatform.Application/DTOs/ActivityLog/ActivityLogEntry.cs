using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.ActivityLog;

public class ActivityLogEntry
{
    public string EntityType { get; set; } = default!;
    public Guid EntityId { get; set; }

    public ActivityType ActivityType { get; set; }

    public string Summary { get; set; } = default!;
    public string? Details { get; set; }

    public Guid PerformedByUserId { get; set; }
}
