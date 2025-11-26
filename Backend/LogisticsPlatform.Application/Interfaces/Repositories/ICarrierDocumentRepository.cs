using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface ICarrierDocumentRepository
    {
        Task AddAsync(CarrierDocument doc);
        Task UpdateAsync(CarrierDocument doc);
        Task DeleteAsync(CarrierDocument doc);
        Task<CarrierDocument?> GetByIdAsync(Guid id);
        Task<IEnumerable<CarrierDocument>> GetByCarrierAsync(Guid carrierId);
        Task SaveChangesAsync();
    }
}
