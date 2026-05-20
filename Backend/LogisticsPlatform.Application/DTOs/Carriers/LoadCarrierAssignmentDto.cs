using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Carriers;

public class LoadCarrierAssignmentDto
{
    public Guid Id { get; set; }
    public Guid LoadId { get; set; }
    public Guid CarrierId { get; set; }
    public string CarrierName { get; set; } = string.Empty;
    public decimal? OfferedRate { get; set; }
    public string? Currency { get; set; }
    public string? RateConfirmationNumber { get; set; }
    public string? TenderMethod { get; set; }
    public string? TenderNotes { get; set; }
    public DateTime? TenderExpiresAt { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTime TenderedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public string? AcceptedByName { get; set; }
    public string? AcceptedByEmail { get; set; }
    public string? AcceptedByPhone { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? RejectedReason { get; set; }
    public bool IsActive { get; set; }
    public string? TenderEmailTo { get; set; }
    public DateTime? TenderEmailSentAt { get; set; }
}
