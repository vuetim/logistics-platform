using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    /// <summary>
    /// Customer invoice service 
    /// - GetAsync(loadId) => Read existing invoice for that load
    /// - CreateAsync(...) => Explicit invoice creation
    /// </summary>
    public interface ICustomerInvoiceService
    {
        /// <summary>
        /// Gets the customer invoice for a given load.
        /// </summary>
        Task<CustomerInvoiceDto> GetAsync(Guid loadId);

        /// <summary>
        /// Lists existing invoices only. Does not auto-create invoices.
        /// </summary>
        Task<List<CustomerInvoiceDto>> ListAsync();

        /// <summary>
        /// Manually creates a customer invoice for a load.
        /// </summary>
        Task<CustomerInvoiceDto> CreateAsync(Guid loadId, CreateInvoiceDto dto, Guid userId);

        /// <summary>
        /// Updates invoice status (Draft, Sent, Paid, etc.).
        /// </summary>
        Task UpdateStatusAsync(Guid invoiceId, InvoiceStatus status, Guid userId);
        Task<CustomerInvoiceDto> RecordPaymentAsync(Guid invoiceId, RecordInvoicePaymentDto dto, Guid userId);

        /// <summary>
        /// Returns the full entity – needed for PDF generation, emails, etc.
        /// </summary>
        Task<CustomerInvoice> GetByIdAsync(Guid invoiceId);

        /// <summary>
        /// Persists the PDF URL after PDF is generated & stored.
        /// </summary>
        Task UpdatePdfUrlAsync(Guid invoiceId, string pdfUrl);
    }
}
