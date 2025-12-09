using LogisticsPlatform.Domain.Entities;

public interface ILoadItemRepository
{
    Task<LoadItem?> GetByIdAsync(Guid loadId, Guid itemId);
    Task<IEnumerable<LoadItem>> GetByLoadIdAsync(Guid loadId);
    Task AddAsync(LoadItem item);
    Task DeleteAsync(LoadItem item);
    Task SaveChangesAsync();
}
