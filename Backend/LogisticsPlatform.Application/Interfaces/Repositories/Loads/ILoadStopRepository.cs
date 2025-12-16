using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads;

public interface ILoadStopRepository
{
    Task AddAsync(LoadStop stop);
    Task UpdateAsync(LoadStop stop);
    Task DeleteAsync(LoadStop stop);
    Task<LoadStop?> GetByIdWithLoadAsync(Guid id);
    Task<LoadStop?> GetByIdAsync(Guid id);
    Task<List<LoadStop>> GetByLoadIdAsync(Guid loadId);
    Task<List<LoadStop>> GetEnRouteStopsWithLoadAsync();

}
