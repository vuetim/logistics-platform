using LogisticsPlatform.Domain.Entities.Financial;

public interface ICustomerInvoiceRepository
{
    Task<CustomerInvoice?> GetByLoadIdAsync(Guid loadId);
    Task<CustomerInvoice?> GetByIdAsync(Guid id);
    Task AddAsync(CustomerInvoice invoice);
    Task SaveChangesAsync();
}
