using LogisticsPlatform.Domain.Enums;

public class OrderRouteDto
{
    public Guid Id { get; set; }

    public int Sequence { get; set; }
    public StopType StopType { get; set; }

    public string LocationName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public DateTime? PlannedArrivalFrom { get; set; }
    public DateTime? PlannedArrivalTo { get; set; }

    public bool HasTime { get; set; }
    public bool CopyToLoad { get; set; }
    public DateTime? AppointmentFrom { get; set; }
    public DateTime? AppointmentTo { get; set; }
    public string? StopReference { get; set; }      // pickup / delivery ref
    public string? AppointmentNumber { get; set; }  // known at planning

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

}
