using LogisticsPlatform.Application.DTOs.Customers;

public interface ICustomerService
{
    Task<IEnumerable<CustomerListItemDto>> GetAllAsync();

    Task<CustomerListItemDto?> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(CreateCustomerDto dto);

    Task<Guid> CreateFullAsync(CreateCustomerFullDto dto, Guid userId);

    Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto);

    Task<bool> DeleteAsync(Guid id);

    Task<CustomerDetailsDto?> GetDetailsAsync(Guid id);
}
