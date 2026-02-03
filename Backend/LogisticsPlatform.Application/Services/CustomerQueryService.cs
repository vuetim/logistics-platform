using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;

public class CustomerQueryService : ICustomerQueryService
{
    private readonly ICustomerQueryRepository _repo;

    public CustomerQueryService(ICustomerQueryRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<CustomerListItemDto>> GetPagedAsync(CustomersQueryParameters parameters)
    {
        return await _repo.GetPagedAsync(parameters);
    }
}
