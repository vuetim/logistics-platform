using LogisticsPlatform.Application.DTOs.Search;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Search;

public interface IGlobalSearchRepository
{
    Task<List<GlobalSearchResultDto>> SearchLoadsAsync(string query, int take);
    Task<List<GlobalSearchResultDto>> SearchCustomersAsync(string query, int take);
}
