using LogisticsPlatform.Domain.Entities;

public interface ILoadItemRepository
{
    Task<LoadItem?> GetByIdAsync(Guid loadId, Guid itemId);
    Task SaveChangesAsync();
}
