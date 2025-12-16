using LogisticsPlatform.Application.DTOs.Orders;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderItemService
{
    Task<Guid> AddAsync(Guid orderId, CreateOrderItemDto dto, Guid userId);

    Task<List<OrderItemDto>> GetByOrderIdAsync(Guid orderId);
}
