using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderExternalIdRepository
    {
        Task<IEnumerable<OrderExternalId>> GetByOrderAsync(Guid orderId);
        Task<OrderExternalId?> GetByIdAsync(Guid id);
        Task AddAsync(OrderExternalId externalId);
        void Update(OrderExternalId externalId);
        void Remove(OrderExternalId externalId);
        Task SaveChangesAsync();
    }
}
