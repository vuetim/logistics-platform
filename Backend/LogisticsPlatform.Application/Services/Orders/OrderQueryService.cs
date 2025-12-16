using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Application.Interfaces.Services.Orders;

namespace LogisticsPlatform.Application.Services
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly IOrderQueryRepository _repo;

        public OrderQueryService(IOrderQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<OrderListDto>> GetPagedAsync(OrderQueryParameters parameters)
        {
            // Guardrails – identik me Load
            if (parameters.PageSize > 100)
                parameters.PageSize = 100;

            return await _repo.GetPagedAsync(parameters);
        }

        public async Task<OrderDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await _repo.GetDetailsAsync(id);
        }
    }
}
