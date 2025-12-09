using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.DTOs.Pagination;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface IOrderQueryService
    {
        Task<PagedResult<OrderListDto>> GetPagedAsync(OrderQueryParameters parameters);
        Task<OrderDetailsDto?> GetDetailsAsync(Guid id);
    }
}
