using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadService
    {
        Task<Guid> CreateAsync(CreateLoadDto dto, Guid userId);
        Task UpdateAsync(Guid id, UpdateLoadDto dto, Guid userId);
        Task ChangeStatusAsync(Guid id, LoadStatus newStatus, Guid userId);
        Task ArchiveAsync(Guid id, Guid userId);
        Task<Guid> CreateFromOrderAsync(
       CreateLoadFromOrderDto dto,
       Guid userId);
        Task DispatchAsync(Guid loadId, DispatchLoadDto dto, Guid userId);

    }
}
