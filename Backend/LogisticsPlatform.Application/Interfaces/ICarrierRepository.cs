using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface ICarrierRepository
    {
        Task<IEnumerable<Carrier>> GetAllAsync();
        Task<Carrier?> GetByIdAsync(Guid id);
        Task AddAsync(Carrier carrier);
        Task UpdateAsync(Carrier carrier);
        Task DeleteAsync(Carrier carrier);
        Task SaveChangesAsync();
    }
}
