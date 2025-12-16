using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

public class DelayResponsibility : BaseEntity
{
    public Guid LoadId { get; set; }
    public Guid LoadStopId { get; set; }

    public DelayResponsibilityType Responsibility { get; set; }
    // Carrier | Shipper |  | Unknown

    public string? Reason { get; set; }

    public bool IsFinal { get; set; } = false;

    public Guid AssignedByUserId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
