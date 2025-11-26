using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task AssignRoleAsync(AssignRoleDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
        Task UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task DeleteUserAsync(Guid id);


    }
}
