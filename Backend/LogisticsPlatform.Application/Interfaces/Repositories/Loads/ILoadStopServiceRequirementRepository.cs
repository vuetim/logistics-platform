using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads;

public interface ILoadStopServiceRequirementRepository
{
    Task<IReadOnlyList<LoadStopServiceRequirement>> GetByStopAsync(Guid stopId);
    Task AddAsync(LoadStopServiceRequirement requirement);
    Task<LoadStopServiceRequirement?> GetByIdForStopWithLoadAsync(Guid stopId, Guid serviceId);
    Task DeleteAsync(LoadStopServiceRequirement requirement);
    Task SaveChangesAsync();
}
