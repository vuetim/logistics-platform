using LogisticsPlatform.Application.DTOs.Carriers.Addresses;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierAddressService
    {
        Task<CarrierAddress> CreateAsync(CreateCarrierAddressDto dto);
        Task<CarrierAddress?> UpdateAsync(Guid id, UpdateCarrierAddressDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CarrierAddress>> GetByCarrierAsync(Guid carrierId);
    }
}
