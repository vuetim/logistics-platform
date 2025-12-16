using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem item);
    }
}
