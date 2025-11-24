using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task AssignRoleAsync(AssignRoleDto dto);

    }
}
