using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Interfaces.Services.Users;

public interface IUserManagementService
{
    Task<List<UserDto>> GetAllAsync(Guid currentUserId);

    Task<UserDto?> GetByIdAsync(Guid id, Guid currentUserId);

    Task UpdateAsync(Guid id, UpdateUserDto dto, Guid currentUserId);

    Task AssignRoleAsync(AssignRoleDto dto, Guid currentUserId);

    Task SetPermissionAsync(
        Guid targetUserId,
        Permission permission,
        bool? isAllowed,
        Guid currentUserId
    );
    Task<List<UserPermissionStateDto>> GetPermissionsAsync(
           Guid targetUserId,
           Guid currentUserId
       );

    Task DisableAsync(Guid userId, Guid currentUserId);

    Task DeleteAsync(Guid id, Guid currentUserId);
}
