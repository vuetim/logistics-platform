using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;

namespace LogisticsPlatform.Application.Interfaces.Services;

public interface ILoadDocumentService
{
    Task AddAsync(Guid loadId, CreateLoadDocumentDto dto);
    Task<IEnumerable<LoadDocumentDto>> GetByLoadAsync(Guid loadId);
    Task DeleteAsync(Guid id);
}
