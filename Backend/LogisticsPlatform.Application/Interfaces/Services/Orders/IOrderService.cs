using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(CreateOrderDto dto, Guid userId);

        Task UpdateAsync(Guid id, UpdateOrderDto dto);

        Task ChangeStatusAsync(Guid id, OrderStatus status);

        Task<OrderDetailsDto?> GetDetailsAsync(Guid id);
    }

}
