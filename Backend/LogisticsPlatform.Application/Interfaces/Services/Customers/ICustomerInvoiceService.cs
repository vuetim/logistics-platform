using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    /// <summary>
    /// Customer invoice service – Turvo-style:
    /// - GetAsync(loadId) => Get or Auto-Create Draft Invoice for that load
    /// - CreateAsync(...)  => Manual create (nëse don explicit)
    /// </summary>
    public interface ICustomerInvoiceService
    {
        /// <summary>
        /// Gets the customer invoice for a given load.
        /// If none exists, automatically generates a draft invoice from the load
        /// (CustomerRate + customer accessorials).
        /// </summary>
        Task<CustomerInvoiceDto> GetAsync(Guid loadId);

        /// <summary>
        /// Manually creates a customer invoice for a load.
        /// Usually used only if you want explicit creation, besides the auto GetOrCreate.
        /// </summary>
        Task<CustomerInvoiceDto> CreateAsync(Guid loadId, CreateInvoiceDto dto, Guid userId);

        /// <summary>
        /// Updates invoice status (Draft, Sent, Paid, etc.).
        /// </summary>
        Task UpdateStatusAsync(Guid invoiceId, InvoiceStatus status, Guid userId);

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
