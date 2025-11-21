using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
    }
}
