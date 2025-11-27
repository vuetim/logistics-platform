using LogisticsPlatform.Application.DTOs.Carriers.Documents;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICarrierDocumentService
    {
        Task<IEnumerable<CarrierDocument>> GetByCarrierAsync(Guid carrierId);
        Task<CarrierDocument> CreateAsync(Guid userId, CreateCarrierDocumentDto dto);
        Task<CarrierDocument?> UpdateAsync(Guid id, UpdateCarrierDocumentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
