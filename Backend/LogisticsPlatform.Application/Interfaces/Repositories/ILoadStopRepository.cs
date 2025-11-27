using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories;

public interface ILoadStopRepository
{
    Task AddAsync(LoadStop stop);
    Task UpdateAsync(LoadStop stop);
    Task DeleteAsync(LoadStop stop);

    Task<LoadStop?> GetByIdAsync(Guid id);
    Task<List<LoadStop>> GetByLoadIdAsync(Guid loadId);
}
