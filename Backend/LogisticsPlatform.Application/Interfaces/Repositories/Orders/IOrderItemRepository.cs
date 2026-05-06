using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem item);
        Task<OrderItem?> GetByIdAsync(Guid id);
        void Update(OrderItem item);
        void Remove(OrderItem item);
    }
}
