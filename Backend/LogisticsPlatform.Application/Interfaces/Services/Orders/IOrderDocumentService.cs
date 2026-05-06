using LogisticsPlatform.Application.DTOs.Orders.Documents;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders
{
    public interface IOrderDocumentService
    {
        Task<IReadOnlyList<OrderDocumentDto>> GetByOrderAsync(Guid orderId, Guid userId);
        Task<OrderDocumentDto> CreateAsync(Guid orderId, CreateOrderDocumentDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
