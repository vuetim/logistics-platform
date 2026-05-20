using LogisticsPlatform.Application.DTOs.Search;

namespace LogisticsPlatform.Application.Interfaces.Services.Search;

public interface IGlobalSearchService
{
    Task<List<GlobalSearchResultDto>> SearchAsync(string query, Guid userId, int take = 8);
}
