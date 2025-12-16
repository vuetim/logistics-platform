using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Carriers
{
    public interface ICarrierDocumentQueryRepository
    {
        Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters);
    }
}
