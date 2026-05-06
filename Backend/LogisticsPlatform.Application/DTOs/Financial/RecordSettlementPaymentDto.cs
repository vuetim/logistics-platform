namespace LogisticsPlatform.Application.DTOs.Financial;

public class RecordSettlementPaymentDto
{
    public decimal AmountPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
}
