using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;

namespace LogisticsPlatform.Application.Interfaces.Repositories;

public interface ILoadQueryRepository
{
    Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters);
    Task<LoadDetailsDto?> GetDetailsAsync(Guid id);
}
