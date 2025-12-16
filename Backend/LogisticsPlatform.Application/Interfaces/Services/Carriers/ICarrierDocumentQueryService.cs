using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierDocumentQueryService
    {
        Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters);
    }
}
