using LogisticsPlatform.Application.DTOs.Search;
using LogisticsPlatform.Application.Interfaces.Repositories.Search;
using LogisticsPlatform.Application.Interfaces.Services.Search;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services.Search;

public class GlobalSearchService : IGlobalSearchService
{
    private readonly IGlobalSearchRepository _search;
    private readonly IPermissionService _permissions;

    public GlobalSearchService(
        IGlobalSearchRepository search,
        IPermissionService permissions)
    {
        _search = search;
        _permissions = permissions;
    }

    public async Task<List<GlobalSearchResultDto>> SearchAsync(string query, Guid userId, int take = 8)
    {
        query = query.Trim();
        if (query.Length < 2) return new List<GlobalSearchResultDto>();

        take = Math.Clamp(take, 1, 20);
        var results = new List<GlobalSearchResultDto>();

        if (await _permissions.HasPermissionAsync(userId, Permission.Load_View))
        {
            results.AddRange(await _search.SearchLoadsAsync(query, take));
        }

        if (await _permissions.HasPermissionAsync(userId, Permission.Customer_View) && results.Count < take)
        {
            results.AddRange(await _search.SearchCustomersAsync(query, take - results.Count));
        }

        return results.Take(take).ToList();
    }
}
