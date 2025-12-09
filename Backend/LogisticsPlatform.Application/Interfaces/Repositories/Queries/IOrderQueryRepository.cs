using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.DTOs.Pagination;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Queries
{
    public interface IOrderQueryRepository
    {
        Task<PagedResult<OrderListDto>> GetPagedAsync(OrderQueryParameters parameters);
        Task<OrderDetailsDto?> GetDetailsAsync(Guid id);
    }
}
