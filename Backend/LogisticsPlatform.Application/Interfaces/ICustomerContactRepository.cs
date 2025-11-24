using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface ICustomerContactRepository
    {
        Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId);
        Task<CustomerContact?> GetByIdAsync(Guid id);

        Task AddAsync(CustomerContact contact);
        Task UpdateAsync(CustomerContact contact);
        Task DeleteAsync(CustomerContact contact);

        Task SaveChangesAsync();
    }
}
