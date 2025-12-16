using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads;

public interface ILoadDocumentRepository
{
    Task AddAsync(LoadDocument document);
    Task<IEnumerable<LoadDocument>> GetByLoadAsync(Guid loadId);
    Task<LoadDocument?> GetByIdAsync(Guid id);
    Task DeleteAsync(LoadDocument document);
    Task SaveChangesAsync();
}
