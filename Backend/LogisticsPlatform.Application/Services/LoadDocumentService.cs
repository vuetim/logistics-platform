using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services;

public class LoadDocumentService : ILoadDocumentService
{
    private readonly ILoadDocumentRepository _documents;
    private readonly ILoadRepository _loads;

    public LoadDocumentService(
        ILoadDocumentRepository documents,
        ILoadRepository loads)
    {
        _documents = documents;
        _loads = loads;
    }

    public async Task AddAsync(Guid loadId, CreateLoadDocumentDto dto)
    {
        var load = await _loads.GetByIdAsync(loadId)
                   ?? throw new Exception("Load not found");

        var document = new LoadDocument
        {
            LoadId = loadId,
            DocumentType = dto.DocumentType,
            FileUrl = dto.FileUrl
        };

        await _documents.AddAsync(document);
        await _documents.SaveChangesAsync();
    }

    public async Task<IEnumerable<LoadDocumentDto>> GetByLoadAsync(Guid loadId)
    {
        var docs = await _documents.GetByLoadAsync(loadId);

        return docs.Select(d => new LoadDocumentDto
        {
            Id = d.Id,
            DocumentType = d.DocumentType,
            FileUrl = d.FileUrl,
            CreatedAt = d.CreatedAt
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        var doc = await _documents.GetByIdAsync(id)
                  ?? throw new Exception("Document not found");

        await _documents.DeleteAsync(doc);
        await _documents.SaveChangesAsync();
    }
}
