using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories;

public interface ILoadQueryRepository
{
    Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters);
    Task<Load?> GetByIdAsync(Guid id);
}
