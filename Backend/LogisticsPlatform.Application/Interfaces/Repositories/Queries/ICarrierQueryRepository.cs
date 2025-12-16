using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Queries
{
    public interface ICarrierQueryRepository
    {
        Task<PagedResult<Carrier>> GetPagedAsync(CarrierQueryParameters parameters);
    }
}
