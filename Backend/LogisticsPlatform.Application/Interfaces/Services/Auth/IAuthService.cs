using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task AssignRoleAsync(AssignRoleDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
        Task UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task<LoginResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<Guid?> ForgotPasswordAsync(string email);
        Task<Guid> ResetPasswordAsync(string token, string newPassword);
        Task DeleteUserAsync(Guid id);


    }
}
