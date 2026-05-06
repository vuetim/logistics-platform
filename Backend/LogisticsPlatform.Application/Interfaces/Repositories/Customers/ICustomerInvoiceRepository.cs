using LogisticsPlatform.Domain.Entities.Financial;

public interface ICustomerInvoiceRepository
{
    Task<CustomerInvoice?> GetByLoadIdAsync(Guid loadId);
    Task<CustomerInvoice?> GetByIdAsync(Guid id);
    Task<List<CustomerInvoice>> ListAsync();
    Task DeleteLineItemsByInvoiceIdAsync(Guid invoiceId);
    Task AddLineItemsAsync(IEnumerable<CustomerInvoiceLineItem> lineItems);
    Task AddAsync(CustomerInvoice invoice);
    Task SaveChangesAsync();
}
