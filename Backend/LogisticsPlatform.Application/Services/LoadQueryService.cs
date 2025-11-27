using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;

namespace LogisticsPlatform.Application.Services
{
    public class LoadQueryService : ILoadQueryService
    {
        private readonly ILoadQueryRepository _repo;

        public LoadQueryService(ILoadQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters)
        {
            // vend për guardrails, defaults, security
            if (parameters.PageSize > 100)
                parameters.PageSize = 100;

            return await _repo.GetPagedAsync(parameters);
        }

        public async Task<LoadDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await _repo.GetDetailsAsync(id);
        }
    }
}
