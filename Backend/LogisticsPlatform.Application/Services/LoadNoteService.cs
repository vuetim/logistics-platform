using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.LoadNote;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services
{
    public class LoadNoteService : ILoadNoteService
    {
        private readonly ILoadNoteRepository _repo;
        private readonly IUserRepository _users;
        private readonly IPermissionService _permission;
        private readonly INotificationService _notifications;

        public LoadNoteService(
            ILoadNoteRepository repo,
            IUserRepository users,
            IPermissionService permission,
            INotificationService notifications)
        {
            _repo = repo;
            _users = users;
            _permission = permission;
            _notifications = notifications;
        }

        public async Task AddAsync(Guid loadId, CreateLoadNoteDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            // ✅ permission check
            var permission = dto.IsInternal
                ? Permission.LoadNote_Create_Internal
                : Permission.LoadNote_Create_Public;

            if (!await _permission.HasPermissionAsync(userId, permission))
                throw new ForbiddenException("You are not allowed to add this note.");

            var note = new LoadNote
            {
                LoadId = loadId,
                Message = dto.Message,
                IsInternal = dto.IsInternal,
                CreatedByUserId = userId
            };

            await _repo.AddAsync(note);
            await _notifications.NotifyLoadNoteAddedAsync(loadId, userId, dto.Message);
        }

        public async Task<List<LoadNoteDto>> GetByLoadAsync(Guid loadId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            var notes = await _repo.GetByLoadIdAsync(loadId);

            // ✅ hide internal notes if no permission
            if (!await _permission.HasPermissionAsync(userId, Permission.LoadNote_View))
            {
                notes = notes.Where(n => !n.IsInternal).ToList();
            }

            return notes.Select(n => new LoadNoteDto
            {
                Id = n.Id,
                Message = n.Message,
                CreatedByName = n.CreatedByUser.FullName,
                CreatedAt = n.CreatedAt,
                IsInternal = n.IsInternal
            }).ToList();
        }
    }
}
