using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

public class CarrierStopPerformance : BaseEntity
{
    public Guid CarrierId { get; set; }
    public Guid LoadId { get; set; }
    public Guid LoadStopId { get; set; }

    public StopType StopType { get; set; }

    public bool IsOnTime { get; set; }
    public bool IsLate { get; set; }
    public int? MinutesLate { get; set; }

    public DateTime PlannedFrom { get; set; }
    public DateTime PlannedTo { get; set; }
    public DateTime ActualArrival { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
