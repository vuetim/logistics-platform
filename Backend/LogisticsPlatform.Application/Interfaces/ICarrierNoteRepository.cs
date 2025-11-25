using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface ICarrierNoteRepository
    {
        Task AddAsync(CarrierNote note);
        Task UpdateAsync(CarrierNote note);
        Task DeleteAsync(CarrierNote note);
        Task<CarrierNote?> GetByIdAsync(Guid id);
        Task<IEnumerable<CarrierNote>> GetByCarrierIdAsync(Guid carrierId);
        Task SaveChangesAsync();
    }
}
