public interface ICustomerAddressRepository
{
    Task<IEnumerable<CustomerAddress>> GetByCustomerAsync(Guid customerId);
    Task<CustomerAddress?> GetByIdAsync(Guid id);
    Task AddAsync(CustomerAddress address);
    Task UpdateAsync(CustomerAddress address);
    Task DeleteAsync(CustomerAddress address);
    Task SaveChangesAsync();
}
