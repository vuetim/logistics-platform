using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(CreateOrderDto dto, Guid userId);
        Task UpdateAsync(Guid id, UpdateOrderDto dto, Guid userId);
        Task ChangeStatusAsync(Guid id, OrderStatus newStatus, Guid userId);
    }
}
