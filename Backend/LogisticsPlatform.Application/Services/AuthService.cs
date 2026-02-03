using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Common;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Security;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Interfaces.Services.Auth;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Options;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace LogisticsPlatform.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordResetTokenRepository _passwordResetTokens;
    private readonly IEmailService _email;
    private readonly IAuthAuditService _audit;
    private readonly IJwtProvider _jwt;
    private readonly IPasswordHasher _hasher;
    private readonly IClock _clock;
    private readonly JwtOptions _jwtOptions;
    private readonly FrontendOptions _frontend;

    public AuthService(
        IUserRepository users,
        IRefreshTokenRepository refreshTokens,
        IPasswordResetTokenRepository passwordResetTokens,
        IEmailService email,
        IAuthAuditService audit,
        IJwtProvider jwt,
        IPasswordHasher hasher,
        IClock clock,
        IOptions<JwtOptions> jwtOptions,
        IOptions<FrontendOptions> frontendOptions)
    {
        _users = users;
        _refreshTokens = refreshTokens;
        _passwordResetTokens = passwordResetTokens;
        _email = email;
        _audit = audit;
        _jwt = jwt;
        _hasher = hasher;
        _clock = clock;
        _jwtOptions = jwtOptions.Value;
        _frontend = frontendOptions.Value;
    }

    // =========================
    // REGISTER
    // =========================
    public async Task RegisterAsync(RegisterDto dto)
    {
        var existing = await _users.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new Exception("Email already exists");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = _hasher.Hash(dto.Password),
            IsActive = true
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        await _audit.LogAsync(user.Id, "Auth.Register.Success", ipAddress: null, userAgent: null);
    }

    // =========================
    // LOGIN
    // =========================
    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, string? ipAddress, string? userAgent)
    {
        var user = await _users.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            await _audit.LogAsync(null, "Auth.Login.Failed", ipAddress: ipAddress, userAgent: userAgent);
            throw new Exception("Invalid credentials");
        }

        if (!user.IsActive)
        {
            await _audit.LogAsync(user.Id, "Auth.Login.Failed.Disabled", ipAddress: ipAddress, userAgent: userAgent);
            throw new Exception("User is disabled");
        }

        if (!_hasher.Verify(dto.Password, user.PasswordHash))
        {
            await _audit.LogAsync(user.Id, "Auth.Login.Failed.InvalidPassword", ipAddress: ipAddress, userAgent: userAgent);
            throw new Exception("Invalid credentials");
        }

        var accessToken = await _jwt.GenerateAccessTokenAsync(user);
        var refreshTokenValue = GenerateRefreshToken();

        var refreshDays = dto.RememberMe
            ? _jwtOptions.RefreshTokenDaysRememberMe
            : _jwtOptions.RefreshTokenDays;

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = _clock.UtcNow.AddDays(refreshDays)
        };

        await _refreshTokens.AddAsync(refreshToken);
        await _refreshTokens.SaveChangesAsync();

        await _audit.LogAsync(user.Id, "Auth.Login.Success", ipAddress: ipAddress, userAgent: userAgent);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }

    // =========================
    // REFRESH
    // =========================
    public async Task<LoginResponseDto> RefreshAsync(string refreshToken, string? ipAddress, string? userAgent)
    {
        var token = await _refreshTokens.GetAsync(refreshToken)
            ?? throw new Exception("Invalid refresh token");

        if (token.IsRevoked || token.ExpiresAt < _clock.UtcNow)
        {
            await _audit.LogAsync(token.UserId, "Auth.Refresh.Failed.Expired", ipAddress: ipAddress, userAgent: userAgent);
            throw new Exception("Refresh token expired");
        }

        // rotate token
        token.IsRevoked = true;

        var user = token.User ?? throw new Exception("User not found");

        var newAccess = await _jwt.GenerateAccessTokenAsync(user);
        var newRefresh = GenerateRefreshToken();

        await _refreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefresh,
            ExpiresAt = _clock.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        });

        await _refreshTokens.SaveChangesAsync();

        await _audit.LogAsync(user.Id, "Auth.Refresh.Success", ipAddress: ipAddress, userAgent: userAgent);

        return new LoginResponseDto
        {
            AccessToken = newAccess,
            RefreshToken = newRefresh
        };
    }

    // =========================
    // LOGOUT
    // =========================
    public async Task LogoutAsync(string refreshToken, Guid userId, string? ipAddress, string? userAgent)
    {
        var token = await _refreshTokens.GetAsync(refreshToken);
        if (token == null)
        {
            await _audit.LogAsync(userId, "Auth.Logout.NoToken", ipAddress: ipAddress, userAgent: userAgent);
            return;
        }

        token.IsRevoked = true;
        await _refreshTokens.SaveChangesAsync();

        await _audit.LogAsync(userId, "Auth.Logout.Success", ipAddress: ipAddress, userAgent: userAgent);
    }

    // =========================
    // FORGOT PASSWORD
    // =========================
    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _users.GetByEmailAsync(email);
        if (user == null)
            return; // silent fail

        var rawToken = GenerateSecureToken();
        var hashedToken = TokenHasher.Hash(rawToken);

        var token = new PasswordResetToken
        {
            UserId = user.Id,
            Token = hashedToken,
            ExpiresAt = _clock.UtcNow.AddMinutes(30)
        };

        await _passwordResetTokens.AddAsync(token);
        await _passwordResetTokens.SaveChangesAsync();

        var encodedToken = Uri.EscapeDataString(rawToken);
        var link = $"{_frontend.BaseUrl}/auth/reset-password?token={encodedToken}";

        await _email.SendAsync(
            user.Email,
            "Reset your password",
            $"Click the link to reset your password:\n{link}"
        );

        await _audit.LogAsync(user.Id, "Auth.ForgotPassword.Sent", ipAddress: null, userAgent: null);
    }

    // =========================
    // RESET PASSWORD
    // =========================
    public async Task ResetPasswordAsync(string tokenValue, string newPassword, string? ip, string? ua)
    {
        var decodedToken = Uri.UnescapeDataString(tokenValue);
        var hashedToken = TokenHasher.Hash(decodedToken);

        var token = await _passwordResetTokens.GetValidAsync(hashedToken)
            ?? throw new Exception("Invalid or expired token");

        var user = token.User ?? throw new Exception("User not found");

        user.PasswordHash = _hasher.Hash(newPassword);
        token.IsUsed = true;

        await _refreshTokens.RevokeAllForUserAsync(user.Id);

        await _users.SaveChangesAsync();
        await _passwordResetTokens.SaveChangesAsync();

        await _audit.LogAsync(user.Id, "Auth.ResetPassword.Success", ipAddress: ip, userAgent: ua);
    }

    // =========================
    // Helpers
    // =========================
    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static string GenerateSecureToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public static class TokenHasher
    {
        public static string Hash(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(bytes);
        }
    }
}
