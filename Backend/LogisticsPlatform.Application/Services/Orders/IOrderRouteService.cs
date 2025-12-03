
public interface IOrderRouteService
{
    Task<Guid> CreateAsync(Guid orderId, CreateOrderRouteDto dto);
    Task<List<OrderRouteDto>> GetByOrderIdAsync(Guid orderId);
    Task UpdateAsync(Guid routeId, UpdateOrderRouteDto dto);
    Task DeleteAsync(Guid routeId);
}
