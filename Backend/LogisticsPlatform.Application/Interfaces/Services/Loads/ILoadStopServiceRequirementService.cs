using LogisticsPlatform.Application.DTOs.Loads.LoadStopServices;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface ILoadStopServiceRequirementService
{
    Task<IReadOnlyList<LoadStopServiceDto>> GetByStopAsync(Guid stopId, Guid userId);
    Task<Guid> CreateAsync(Guid stopId, CreateLoadStopServiceRequest request, Guid userId);
    Task DeleteAsync(Guid stopId, Guid serviceId, Guid userId);
}
