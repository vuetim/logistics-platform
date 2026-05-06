namespace LogisticsPlatform.Application.DTOs.Financial;

public class RecordInvoicePaymentDto
{
    public decimal AmountPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
}
