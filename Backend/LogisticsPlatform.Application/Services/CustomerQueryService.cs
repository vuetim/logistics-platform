using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;

public class CustomerQueryService : ICustomerQueryService
{
    private readonly ICustomerQueryRepository _repo;

    public CustomerQueryService(ICustomerQueryRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<Customer>> GetPagedAsync(QueryParameters parameters)
    {
        return await _repo.GetPagedAsync(parameters);
    }
}
