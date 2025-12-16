using LogisticsPlatform.Domain.Entities;

public interface IOrderRouteRepository
{
    Task AddAsync(OrderRoute route);
    Task<OrderRoute?> GetByIdAsync(Guid id);
    Task<List<OrderRoute>> GetByOrderIdAsync(Guid orderId);
    Task UpdateAsync(OrderRoute route);
    Task DeleteAsync(OrderRoute route);
    Task SaveChangesAsync();
}
