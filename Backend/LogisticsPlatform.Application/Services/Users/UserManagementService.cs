using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _users;
    private readonly IAuthorizationService _authz;

    public UserManagementService(
        IUserRepository users,
        IAuthorizationService authz)
    {
        _users = users;
        _authz = authz;
    }

    public async Task<List<UserDto>> GetAllAsync(Guid currentUserId)
    {
        var currentUser = await _users.GetByIdAsync(currentUserId)
            ?? throw new Exception("User not found");

        if (!_authz.HasPermission(currentUser, Permission.User_View_All))
            throw new Exception("Forbidden");

        var users = await _users.GetAllAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Roles = u.UserRoles?.Select(r => r.Role.Name).ToList() ?? new()
        }).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, Guid currentUserId)
    {
        var currentUser = await _users.GetByIdAsync(currentUserId)
            ?? throw new Exception("User not found");

        var target = await _users.GetByIdAsync(id);
        if (target == null) return null;

        if (!_authz.HasPermission(currentUser, Permission.User_View_Self, target))
            throw new Exception("Forbidden");

        return new UserDto
        {
            Id = target.Id,
            FullName = target.FullName,
            Email = target.Email,
            Roles = target.UserRoles?.Select(r => r.Role.Name).ToList() ?? new()
        };
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto dto, Guid currentUserId)
    {
        var currentUser = await _users.GetByIdAsync(currentUserId)
            ?? throw new Exception("User not found");

        var target = await _users.GetByIdAsync(id)
            ?? throw new Exception("User not found");

        if (!_authz.HasPermission(currentUser, Permission.User_Update, target))
            throw new Exception("Forbidden");

        target.FullName = dto.FullName;
        target.Email = dto.Email;

        await _users.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, Guid currentUserId)
    {
        var currentUser = await _users.GetByIdAsync(currentUserId)
            ?? throw new Exception("User not found");

        if (!_authz.HasPermission(currentUser, Permission.User_Delete))
            throw new Exception("Forbidden");

        var target = await _users.GetByIdAsync(id)
            ?? throw new Exception("User not found");

        await _users.DeleteAsync(target);
        await _users.SaveChangesAsync();
    }
}
