using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierService
    {
        Task<IEnumerable<Carrier>> GetAllAsync();
        Task<Carrier?> GetByIdAsync(Guid id);
        Task<Carrier> CreateAsync(CreateCarrierDto dto);
        Task<Carrier?> UpdateAsync(Guid id, UpdateCarrierDto dto);
        Task<bool> DeleteAsync(Guid id);

    }
}
