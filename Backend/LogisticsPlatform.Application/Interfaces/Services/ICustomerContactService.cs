using LogisticsPlatform.Application.DTOs.Customers.Contacts;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICustomerContactService
    {
        Task<CustomerContact> CreateAsync(CreateCustomerContactDto dto);
        Task<CustomerContact?> UpdateAsync(Guid id, UpdateCustomerContactDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId);
    }
}
