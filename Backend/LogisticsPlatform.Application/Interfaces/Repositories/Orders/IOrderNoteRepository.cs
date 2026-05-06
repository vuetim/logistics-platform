using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Orders
{
    public interface IOrderNoteRepository
    {
        Task<IEnumerable<OrderNote>> GetByOrderAsync(Guid orderId);
        Task<OrderNote?> GetByIdAsync(Guid id);
        Task AddAsync(OrderNote note);
        void Update(OrderNote note);
        void Remove(OrderNote note);
    }
}
