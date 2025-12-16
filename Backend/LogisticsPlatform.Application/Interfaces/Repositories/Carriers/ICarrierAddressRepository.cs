using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Carriers
{
    public interface ICarrierAddressRepository
    {
        Task AddAsync(CarrierAddress address);
        Task UpdateAsync(CarrierAddress address);
        Task DeleteAsync(CarrierAddress address);
        Task<CarrierAddress?> GetByIdAsync(Guid id);
        Task<IEnumerable<CarrierAddress>> GetByCarrierAsync(Guid carrierId);
        Task<List<CarrierAddress>> GetPrimaryByCarrierAsync(Guid carrierId);

        Task SaveChangesAsync();
    }
}
