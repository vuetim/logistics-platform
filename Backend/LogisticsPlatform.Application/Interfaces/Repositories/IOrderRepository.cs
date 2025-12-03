using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetByIdWithRoutesAsync(Guid id);

        Task SaveChangesAsync();
    }
}
