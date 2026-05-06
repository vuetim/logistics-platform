using LogisticsPlatform.Application.DTOs.Orders;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderItemService
{
    Task<Guid> AddAsync(Guid orderId, CreateOrderItemDto dto, Guid userId);
    Task UpdateAsync(Guid orderId, Guid itemId, UpdateOrderItemDto dto, Guid userId);
    Task DeleteAsync(Guid orderId, Guid itemId, Guid userId);

    Task<List<OrderItemDto>> GetByOrderIdAsync(Guid orderId);
}
