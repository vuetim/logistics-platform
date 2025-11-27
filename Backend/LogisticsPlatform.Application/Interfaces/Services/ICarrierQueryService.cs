using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICarrierQueryService
    {
        Task<PagedResult<Carrier>> GetPagedAsync(CarrierQueryParameters parameters);
    }
}
