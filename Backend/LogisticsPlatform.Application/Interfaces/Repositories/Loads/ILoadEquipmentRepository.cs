using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads;

public interface ILoadEquipmentRepository
{
    Task AddAsync(LoadEquipment equipment);
    Task UpdateAsync(LoadEquipment equipment);
    Task DeleteAsync(LoadEquipment equipment);

    Task<LoadEquipment?> GetByIdAsync(Guid id);
    Task<IEnumerable<LoadEquipment>> GetByLoadIdAsync(Guid loadId);

    Task SaveChangesAsync();
}
