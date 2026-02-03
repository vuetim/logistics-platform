using LogisticsPlatform.Application.DTOs.Customers.Contacts;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    public interface ICustomerContactService
    {
        Task<CustomerContactDto> CreateAsync(CreateCustomerContactDto dto);
        Task<CustomerContactDto?> UpdateAsync(Guid id, UpdateCustomerContactDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyList<CustomerContactDto>> GetByCustomerAsync(Guid customerId);
    }

}
