using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Customers
{
    public interface ICustomerNoteRepository
    {
        Task<IEnumerable<CustomerNote>> GetByCustomerAsync(Guid customerId);
        Task<CustomerNote?> GetByIdAsync(Guid id);

        Task AddAsync(CustomerNote note);
        Task UpdateAsync(CustomerNote note);
        Task DeleteAsync(CustomerNote note);

        Task SaveChangesAsync();
    }
}
