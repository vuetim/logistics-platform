using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces.Services.Users;

public interface IUserManagementService
{
    Task<List<UserDto>> GetAllAsync(Guid currentUserId);
    Task<UserDto?> GetByIdAsync(Guid id, Guid currentUserId);
    Task UpdateAsync(Guid id, UpdateUserDto dto, Guid currentUserId);
    Task DeleteAsync(Guid id, Guid currentUserId);
}
