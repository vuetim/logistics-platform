using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.LoadNote;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Services
{
    public class LoadNoteService : ILoadNoteService
    {
        private readonly ILoadNoteRepository _repo;
        private readonly IUserRepository _users;
        private readonly IAuthorizationService _auth;

        public LoadNoteService(
            ILoadNoteRepository repo,
            IUserRepository users,
            IAuthorizationService auth)
        {
            _repo = repo;
            _users = users;
            _auth = auth;
        }

        public async Task AddAsync(Guid loadId, CreateLoadNoteDto dto, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            // ✅ permission check
            var permission = dto.IsInternal
                ? Permission.LoadNote_Create_Internal
                : Permission.LoadNote_Create_Public;

            if (!_auth.HasPermission(user, permission))
                throw new ForbiddenException("You are not allowed to add this note.");

            var note = new LoadNote
            {
                LoadId = loadId,
                Message = dto.Message,
                IsInternal = dto.IsInternal,
                CreatedByUserId = userId
            };

            await _repo.AddAsync(note);
        }

        public async Task<List<LoadNoteDto>> GetByLoadAsync(Guid loadId, Guid userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new ForbiddenException("User not found");

            var notes = await _repo.GetByLoadIdAsync(loadId);

            // ✅ hide internal notes if no permission
            if (!_auth.HasPermission(user, Permission.LoadNote_View))
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
