using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface ICarrierQueryRepository
    {
        Task<PagedResult<Carrier>> GetPagedAsync(CarrierQueryParameters parameters);
    }
}
