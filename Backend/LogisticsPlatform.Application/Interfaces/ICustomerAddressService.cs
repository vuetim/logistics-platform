public interface ICustomerAddressService
{
    Task<CustomerAddress> CreateAsync(CreateCustomerAddressDto dto);
    Task<IEnumerable<CustomerAddress>> GetByCustomerAsync(Guid customerId);
    Task<CustomerAddress?> UpdateAsync(Guid id, UpdateCustomerAddressDto dto);
    Task<bool> DeleteAsync(Guid id);
}
