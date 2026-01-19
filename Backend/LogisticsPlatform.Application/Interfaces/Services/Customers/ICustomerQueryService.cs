using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    public interface ICustomerQueryService
    {
        Task<PagedResult<CustomerListItemDto>> GetPagedAsync(CustomersQueryParameters parameters);
    }
}
