public interface ICustomerAddressRepository
{
    Task<IEnumerable<CustomerAddress>> GetByCustomerAsync(Guid customerId);

    Task<CustomerAddress?> GetByIdAsync(Guid id);

    Task<List<CustomerAddress>> GetPrimaryByCustomerAsync(Guid customerId);

    Task AddAsync(CustomerAddress address);

    void Update(CustomerAddress address);

    void Remove(CustomerAddress address);
}
