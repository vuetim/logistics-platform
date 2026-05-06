using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Financial;

public class CarrierSettlementDto
{
    public Guid Id { get; set; }
    public Guid LoadId { get; set; }
    public Guid CarrierId { get; set; }

    public string SettlementNumber { get; set; } = string.Empty;
    public DateTime SettlementDate { get; set; }

    public SettlementStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    public string Notes { get; set; }
    public DateTime? DueDate { get; set; }

    public List<CarrierSettlementLineItemDto> LineItems { get; set; } = new();
    public string? PdfUrl { get; internal set; }
}
