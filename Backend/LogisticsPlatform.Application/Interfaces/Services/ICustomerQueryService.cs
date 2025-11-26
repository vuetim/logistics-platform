using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICustomerQueryService
    {
        Task<PagedResult<Customer>> GetPagedAsync(QueryParameters parameters);
    }
}
