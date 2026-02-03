using LogisticsPlatform.Application.DTOs.Auth;

namespace LogisticsPlatform.Application.Interfaces.Services.Auth;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);

    Task<LoginResponseDto> LoginAsync(
        LoginDto dto,
        string? ipAddress,
        string? userAgent
    );

    Task<LoginResponseDto> RefreshAsync(
        string refreshToken,
        string? ipAddress,
        string? userAgent
    );

    Task LogoutAsync(
        string refreshToken,
        Guid userId,
        string? ipAddress,
        string? userAgent
    );

    Task ForgotPasswordAsync(string email);

    Task ResetPasswordAsync(
        string token,
        string newPassword,
        string? ip,
        string? ua
    );
}
