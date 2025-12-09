using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem item);
    }
}
