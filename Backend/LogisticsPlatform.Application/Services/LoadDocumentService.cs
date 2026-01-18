using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services
{
    public class LoadDocumentService : ILoadDocumentService
    {
        private readonly ILoadDocumentRepository _documents;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;

        public LoadDocumentService(
            ILoadDocumentRepository documents,
            IUserRepository users,
            IPermissionService permission)
        {
            _documents = documents;
            _users = users;
            _permission = permission;
        }

        // UPLOAD DOCUMENT
        public async Task AddAsync(Guid loadId, CreateLoadDocumentDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_Upload))
                throw new ForbiddenException("You are not allowed to upload load documents.");

            var document = new LoadDocument
            {
                LoadId = loadId,
                DocumentType = dto.DocumentType,
                FileUrl = dto.FileUrl,
                IsInternal = dto.IsInternal
            };

            await _documents.AddAsync(document);
            await _documents.SaveChangesAsync();
        }

        // VIEW DOCUMENTS (filtered)
        public async Task<List<LoadDocumentDto>> GetByLoadAsync(Guid loadId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            var docs = await _documents.GetByLoadAsync(loadId);

            // Nëse s'ka permission për internal → shfaq vetëm public
            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_View))
            {
                docs = docs.Where(d => !d.IsInternal).ToList();
            }

            return docs.Select(d => new LoadDocumentDto
            {
                Id = d.Id,
                DocumentType = d.DocumentType,
                FileUrl = d.FileUrl,
                IsInternal = d.IsInternal,
                CreatedAt = d.CreatedAt
            }).ToList();
        }

        // DELETE DOCUMENT
        public async Task DeleteAsync(Guid documentId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            if (!await _permission.HasPermissionAsync(userId, Permission.LoadDocument_Delete))
                throw new ForbiddenException("You are not allowed to delete documents.");

            var document = await _documents.GetByIdAsync(documentId)
                ?? throw new Exception("Document not found");

            await _documents.DeleteAsync(document);
            await _documents.SaveChangesAsync();
        }
    }
}
