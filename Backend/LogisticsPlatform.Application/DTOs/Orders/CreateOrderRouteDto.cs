using LogisticsPlatform.Domain.Enums;

public class CreateOrderRouteDto
{
    public int Sequence { get; set; }
    public StopType StopType { get; set; }   // Pickup / Delivery / Stop

    public string LocationName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }

    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public DateTime? PlannedArrivalFrom { get; set; }
    public DateTime? PlannedArrivalTo { get; set; }

    public AppointmentType AppointmentType { get; set; } = AppointmentType.Appointment;
    public int? FlexMinutes { get; set; }
    public bool HasTime { get; set; } = true;
    public string? TimeZone { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pending;
    public bool AppointmentConfirmed { get; set; }
    public string? AppointmentConfirmationNumber { get; set; }
    public bool CopyToLoad { get; set; } = true;
    public string? StopReference { get; set; }      // pickup / delivery ref
    public string? AppointmentNumber { get; set; }  // known at planning
    public string? PONumbers { get; set; }

    public string? Notes { get; set; }
}
