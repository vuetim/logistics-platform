using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Financial;

public class CustomerInvoiceDto
{
    public Guid Id { get; set; }
    public Guid LoadId { get; set; }
    public Guid CustomerId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }

    public InvoiceType InvoiceType { get; set; }
    public InvoiceStatus Status { get; set; }

    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }

    public List<InvoiceLineItemDto> LineItems { get; set; } = new();
    public string PdfUrl { get; internal set; }
}
