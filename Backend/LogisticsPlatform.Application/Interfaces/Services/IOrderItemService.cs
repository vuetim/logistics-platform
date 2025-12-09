using LogisticsPlatform.Application.DTOs.Orders;

namespace LogisticsPlatform.Application.Interfaces.Services;

public interface IOrderItemService
{
    Task<Guid> AddAsync(Guid orderId, CreateOrderItemDto dto, Guid userId);

    Task<List<OrderItemDto>> GetByOrderIdAsync(Guid orderId);
}
