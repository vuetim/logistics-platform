using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories;

public interface ILoadRepository
{
    Task AddAsync(Load load);
    Task UpdateAsync(Load load);
    Task<Load?> GetByIdAsync(Guid id);
    Task AddLoadOrderAsync(LoadOrder loadOrder);
    Task AddStopAsync(LoadStop stop);

    Task SaveChangesAsync();
}
