using LogisticsPlatform.Application.DTOs.Carriers.Addresses;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierAddressService : ICarrierAddressService
    {
        private readonly ICarrierAddressRepository _addresses;
        private readonly ICarrierRepository _carriers;

        public CarrierAddressService(
            ICarrierAddressRepository addresses,
            ICarrierRepository carriers)
        {
            _addresses = addresses;
            _carriers = carriers;
        }

        public async Task<CarrierAddress> CreateAsync(CreateCarrierAddressDto dto)
        {
            var carrier = await _carriers.GetByIdAsync(dto.CarrierId);
            if (carrier == null)
                throw new Exception("Carrier not found");

            var address = new CarrierAddress
            {
                CarrierId = dto.CarrierId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                Type = dto.Type,
                IsPrimary = dto.IsPrimary
            };

            await _addresses.AddAsync(address);
            await _addresses.SaveChangesAsync();

            return address;
        }

        public async Task<CarrierAddress?> UpdateAsync(Guid id, UpdateCarrierAddressDto dto)
        {
            var address = await _addresses.GetByIdAsync(id);
            if (address == null) return null;

            address.AddressLine1 = dto.AddressLine1 ?? address.AddressLine1;
            address.AddressLine2 = dto.AddressLine2 ?? address.AddressLine2;
            address.City = dto.City ?? address.City;
            address.State = dto.State ?? address.State;
            address.Country = dto.Country ?? address.Country;
            address.PostalCode = dto.PostalCode ?? address.PostalCode;
            address.Type = dto.Type ?? address.Type;

            if (dto.IsPrimary.HasValue)
                address.IsPrimary = dto.IsPrimary.Value;

            address.UpdatedAt = DateTime.UtcNow;

            await _addresses.UpdateAsync(address);
            await _addresses.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = await _addresses.GetByIdAsync(id);
            if (address == null) return false;

            await _addresses.DeleteAsync(address);
            await _addresses.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CarrierAddress>> GetByCarrierAsync(Guid carrierId)
        {
            return await _addresses.GetByCarrierAsync(carrierId);
        }
    }
}
