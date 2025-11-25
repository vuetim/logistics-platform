using LogisticsPlatform.Application.DTOs.Carriers.Contacts;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface ICarrierContactService
    {
        Task<CarrierContact> CreateAsync(CreateCarrierContactDto dto);
        Task<CarrierContact?> UpdateAsync(Guid id, UpdateCarrierContactDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CarrierContact>> GetByCarrierAsync(Guid carrierId);
    }
}
