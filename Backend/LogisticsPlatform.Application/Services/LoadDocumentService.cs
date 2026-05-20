using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using SendGrid.Helpers.Errors.Model;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services
{
    public class LoadDocumentService : ILoadDocumentService
    {
        private readonly ILoadDocumentRepository _documents;
        private readonly ILoadRepository _loads;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;
        private readonly INotificationService _notifications;

        public LoadDocumentService(
            ILoadDocumentRepository documents,
            ILoadRepository loads,
            IUserRepository users,
            IPermissionService permission,
            INotificationService notifications)
        {
            _documents = documents;
            _loads = loads;
            _users = users;
            _permission = permission;
            _notifications = notifications;
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

            if (dto.DocumentType == LoadDocumentType.POD)
            {
                var load = await _loads.GetByIdAsync(loadId)
                    ?? throw new NotFoundException("Load not found.");

                load.PodReceivedAt ??= DateTime.UtcNow;
                load.PodUploadedBy = user.FullName;
            }

            await _documents.SaveChangesAsync();
            await _notifications.NotifyLoadDocumentEventAsync(
                loadId,
                userId,
                $"{dto.DocumentType} document uploaded");
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
            var loadId = document.LoadId;
            var documentType = document.DocumentType;

            await _documents.DeleteAsync(document);
            await _documents.SaveChangesAsync();
            await _notifications.NotifyLoadDocumentEventAsync(
                loadId,
                userId,
                $"{documentType} document deleted");
        }
    }
}
