using LogisticsPlatform.Application.DTOs.Loads;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadItemService
    {
        Task UpdateAsync(
            Guid loadId,
            Guid itemId,
            UpdateLoadItemDto dto,
            Guid userId);
    }
}
