using LogisticsPlatform.Application.DTOs.Customers.Addresses;

public interface ICustomerAddressService
{
    Task<CustomerAddressDto> CreateAsync(CreateCustomerAddressDto dto);

    Task<IReadOnlyList<CustomerAddressDto>> GetByCustomerAsync(Guid customerId);

    Task<CustomerAddressDto?> UpdateAsync(Guid id, UpdateCustomerAddressDto dto);

    Task<bool> DeleteAsync(Guid id);
}
