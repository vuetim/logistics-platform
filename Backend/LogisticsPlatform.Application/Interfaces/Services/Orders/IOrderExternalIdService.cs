using LogisticsPlatform.Application.DTOs.Orders.ExternalIds;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders
{
    public interface IOrderExternalIdService
    {
        Task<IReadOnlyList<OrderExternalIdDto>> GetByOrderAsync(Guid orderId, Guid userId);
        Task<OrderExternalIdDto> CreateAsync(Guid orderId, CreateOrderExternalIdDto dto, Guid userId);
        Task<OrderExternalIdDto?> UpdateAsync(Guid id, UpdateOrderExternalIdDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
