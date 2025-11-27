using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadService
    {
        Task<Guid> CreateAsync(CreateLoadDto dto, Guid createdByUserId);
        Task UpdateAsync(Guid id, UpdateLoadDto dto);
        Task ChangeStatusAsync(Guid id, LoadStatus newStatus);
        Task ArchiveAsync(Guid id);
    }
}
