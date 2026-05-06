using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderDocumentRepository
    {
        Task<IEnumerable<OrderDocument>> GetByOrderAsync(Guid orderId);
        Task<OrderDocument?> GetByIdAsync(Guid id);
        Task AddAsync(OrderDocument document);
        void Remove(OrderDocument document);
        Task SaveChangesAsync();
    }
}
