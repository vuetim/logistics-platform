using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICarrierDocumentQueryService
    {
        Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters);
    }
}
