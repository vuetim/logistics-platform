using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface ICustomerQueryRepository
    {
        Task<PagedResult<Customer>> GetPagedAsync(QueryParameters parameters);
    }
}
