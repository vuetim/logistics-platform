using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadQueryService
    {
        Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters);
        Task<LoadDetailsDto?> GetDetailsAsync(Guid id, Guid userId);
    }
}
