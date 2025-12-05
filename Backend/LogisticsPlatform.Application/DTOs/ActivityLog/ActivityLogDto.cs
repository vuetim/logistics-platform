namespace LogisticsPlatform.Application.DTOs.ActivityLog;

public class ActivityLogDto
{
    public string Action { get; set; } = default!;
    public string? Field { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Details { get; set; }

    public string PerformedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
