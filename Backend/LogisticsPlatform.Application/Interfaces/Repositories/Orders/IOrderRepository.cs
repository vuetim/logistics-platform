using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderRepository
    {
        // Commands

        Task AddAsync(Order order);
        Task UpdateAsync(Order order);

        // Aggregate loading (DDD safe)

        // full aggregate (edit screen / wizard)
        Task<Order?> GetByIdAsync(Guid id);

        // optimized loads (performance)
        Task<Order?> GetByIdWithRoutesAsync(Guid id);
        Task<Order?> GetByIdWithItemsAsync(Guid id);
        Task<Order?> GetByIdWithLoadsAsync(Guid id);

        // Unit of Work
        Task SaveChangesAsync();
    }
}
