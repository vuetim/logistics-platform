namespace LogisticsPlatform.Application.DTOs.Carriers;

public class OpenCarrierOfferDto
{
    public Guid AssignmentId { get; set; }
    public Guid LoadId { get; set; }
    public string LoadNumber { get; set; } = string.Empty;
    public string LoadStatus { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public Guid CarrierId { get; set; }
    public string CarrierName { get; set; } = string.Empty;
    public decimal? OfferedRate { get; set; }
    public string Currency { get; set; } = "USD";
    public string? RateConfirmationNumber { get; set; }
    public string? TenderMethod { get; set; }
    public string? TenderNotes { get; set; }
    public DateTime TenderedAt { get; set; }
    public DateTime? TenderExpiresAt { get; set; }
}
