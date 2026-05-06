using LogisticsPlatform.Application.DTOs.Orders.Notes;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders
{
    public interface IOrderNoteService
    {
        Task<IReadOnlyList<OrderNoteDto>> GetByOrderAsync(Guid orderId, Guid userId);
        Task<OrderNoteDto> CreateAsync(Guid orderId, CreateOrderNoteDto dto, Guid userId);
        Task<OrderNoteDto?> UpdateAsync(Guid id, UpdateOrderNoteDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
