using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierService : ICarrierService
    {
        private readonly ICarrierRepository _carriers;

        public CarrierService(ICarrierRepository carriers)
        {
            _carriers = carriers;
        }

        public async Task<IEnumerable<Carrier>> GetAllAsync()
        {
            return await _carriers.GetAllAsync();
        }

        public async Task<Carrier?> GetByIdAsync(Guid id)
        {
            return await _carriers.GetByIdAsync(id);
        }

        public async Task<Carrier> CreateAsync(CreateCarrierDto dto)
        {
            var carrier = new Carrier
            {
                Name = dto.Name,
                McNumber = dto.McNumber,
                DotNumber = dto.DotNumber,
                Phone = dto.Phone,
                Email = dto.Email,
                Status = dto.Status,
                Rating = dto.Rating,
                PaymentTermsDays = dto.PaymentTermsDays
            };

            await _carriers.AddAsync(carrier);
            await _carriers.SaveChangesAsync();

            return carrier;
        }

        public async Task<Carrier?> UpdateAsync(Guid id, UpdateCarrierDto dto)
        {
            var carrier = await _carriers.GetByIdAsync(id);
            if (carrier == null) return null;

            carrier.Name = dto.Name ?? carrier.Name;
            carrier.McNumber = dto.McNumber ?? carrier.McNumber;
            carrier.DotNumber = dto.DotNumber ?? carrier.DotNumber;
            carrier.Phone = dto.Phone ?? carrier.Phone;
            carrier.Email = dto.Email ?? carrier.Email;
            carrier.Status = dto.Status ?? carrier.Status;
            carrier.PaymentTermsDays = dto.PaymentTermsDays;

            if (dto.Rating.HasValue)
                carrier.Rating = dto.Rating.Value;

            await _carriers.UpdateAsync(carrier);
            await _carriers.SaveChangesAsync();

            return carrier;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var carrier = await _carriers.GetByIdAsync(id);
            if (carrier == null) return false;

            await _carriers.DeleteAsync(carrier);
            await _carriers.SaveChangesAsync();
            return true;
        }
    }
}
