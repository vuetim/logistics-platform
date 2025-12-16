using LogisticsPlatform.Application.DTOs.Carriers.Notes;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierNoteService
    {
        Task<IEnumerable<CarrierNote>> GetByCarrierAsync(Guid carrierId);
        Task<CarrierNote> CreateAsync(Guid userId, CreateCarrierNoteDto dto);
        Task<CarrierNote?> UpdateAsync(Guid id, UpdateCarrierNoteDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
