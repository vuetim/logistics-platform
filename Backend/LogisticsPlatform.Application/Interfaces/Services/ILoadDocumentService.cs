using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;

namespace LogisticsPlatform.Application.Interfaces.Services;

public interface ILoadDocumentService
{
    Task AddAsync(Guid loadId, CreateLoadDocumentDto dto, Guid userId);
    Task<List<LoadDocumentDto>> GetByLoadAsync(Guid loadId, Guid userId);
    Task DeleteAsync(Guid documentId, Guid userId);
}

