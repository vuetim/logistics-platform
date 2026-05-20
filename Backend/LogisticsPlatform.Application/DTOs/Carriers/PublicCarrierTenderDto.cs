namespace LogisticsPlatform.Application.DTOs.Carriers;

public class PublicCarrierTenderDto
{
    public Guid AssignmentId { get; set; }
    public string LoadNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string CarrierName { get; set; } = string.Empty;
    public decimal? OfferedRate { get; set; }
    public string Currency { get; set; } = "USD";
    public string? TenderNotes { get; set; }
    public DateTime? TenderExpiresAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public IReadOnlyList<PublicCarrierTenderStopDto> Stops { get; set; } = [];
}

public class PublicCarrierTenderStopDto
{
    public int Sequence { get; set; }
    public string StopType { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime? PlannedArrivalFrom { get; set; }
    public DateTime? PlannedArrivalTo { get; set; }
}
