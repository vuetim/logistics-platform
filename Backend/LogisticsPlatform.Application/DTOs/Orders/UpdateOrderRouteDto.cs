using LogisticsPlatform.Domain.Enums;

public class UpdateOrderRouteDto
{
    public int? Sequence { get; set; }
    public StopType? StopType { get; set; }

    public string? LocationName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public DateTime? PlannedArrivalFrom { get; set; }
    public DateTime? PlannedArrivalTo { get; set; }
    public string? StopReference { get; set; }      // pickup / delivery ref
    public string? AppointmentNumber { get; set; }  // known at planning

    public bool? HasTime { get; set; }
    public bool? CopyToLoad { get; set; }

    public string? Notes { get; set; }
}
