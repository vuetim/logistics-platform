using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    public interface ICustomerService
    {
        public Task<Customer> CreateFullAsync(CreateCustomerFullDto dto, Guid userId
            );
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer> CreateAsync(CreateCustomerDto dto);
        Task<Customer?> UpdateAsync(Guid id, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
