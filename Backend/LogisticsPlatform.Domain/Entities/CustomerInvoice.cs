//using LogisticsPlatform.Domain.Entities.Financial;
//using LogisticsPlatform.Domain.Enums;

//public class CustomerInvoice
//{
//    public Guid Id { get; set; } = Guid.NewGuid();

//    public Guid LoadId { get; set; }
//    public Guid CustomerId { get; set; }

//    // Core invoice info
//    public string InvoiceNumber { get; set; } = default!;
//    public DateTime InvoiceDate { get; set; }
//    public DateTime DueDate { get; set; }

//    // Extended B2
//    public string? Notes { get; set; }
//    public string? AccountName { get; set; } // TODO: this to work later
//    public InvoiceType InvoiceType { get; set; } = InvoiceType.Customer;
//    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

//    public string? DocumentId { get; set; } // used for PDF generation reference

//    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

//    public decimal TotalAmount { get; set; }

//    public List<CustomerInvoiceLineItem> LineItems { get; set; } = new();
//}
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities.Financial;

public class CustomerInvoice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // lidhja
    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // info bazë
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }

    public InvoiceType InvoiceType { get; set; } = InvoiceType.Customer;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public string? Notes { get; set; }

    // total
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    // AUDIT FIELDS
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    // PDF link 
    public string? PdfUrl { get; set; }
    // line items
    public List<CustomerInvoiceLineItem> LineItems { get; set; } = new();
}
