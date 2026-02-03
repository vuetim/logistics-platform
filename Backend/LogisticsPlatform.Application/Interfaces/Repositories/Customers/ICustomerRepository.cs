using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);

        Task<Customer?> GetDetailsAsync(Guid id); // aggregate

        Task<IEnumerable<Customer>> GetAllAsync();

        Task AddAsync(Customer customer);

        void Update(Customer customer);

       
    }
}
