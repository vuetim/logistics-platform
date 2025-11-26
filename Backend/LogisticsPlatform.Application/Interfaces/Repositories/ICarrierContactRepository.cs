using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface ICarrierContactRepository
    {
        Task AddAsync(CarrierContact contact);
        Task UpdateAsync(CarrierContact contact);
        Task DeleteAsync(CarrierContact contact);
        Task<CarrierContact?> GetByIdAsync(Guid id);
        Task<IEnumerable<CarrierContact>> GetByCarrierAsync(Guid carrierId);
        Task SaveChangesAsync();
    }
}
