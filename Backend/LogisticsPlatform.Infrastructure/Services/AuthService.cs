using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Interfaces.Services.Auth;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LogisticsPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IUserRoleRepository _userRoles;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokens;
        private readonly IPasswordResetTokenRepository _passwordResetTokens;
        private readonly IEmailService _email;
        private readonly IAuthAuditService _audit;
        private readonly IPermissionService _permissions;



        public AuthService(IUserRepository users, IRoleRepository roles, IUserRoleRepository userRoles, IConfiguration config,
            IRefreshTokenRepository refreshTokens, IEmailService email, IPasswordResetTokenRepository passwordResetTokens,IAuthAuditService audit, IPermissionService permissions)
        {
            _users = users;
            _roles = roles;
            _userRoles = userRoles;
            _refreshTokens = refreshTokens;
            _config = config;
            _email = email;
            _passwordResetTokens = passwordResetTokens;
            _audit = audit;
            _permissions = permissions;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var existing = await _users.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true
            };

            await _users.AddAsync(user);
            await _users.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(
        LoginDto dto,
        string? ipAddress,
        string? userAgent)
        {
            var user = await _users.GetByEmailAsync(dto.Email);

            //  USER NOT FOUND
            if (user == null)
            {
                await _audit.LogAsync(
                    null,
                    "Auth.Login.Failed",
                    ipAddress: ipAddress,
                    userAgent: userAgent
                );

                throw new Exception("Invalid credentials");
            }

            //  USER DISABLED
            if (!user.IsActive)
            {
                await _audit.LogAsync(
                    user.Id,
                    "Auth.Login.Failed.Disabled",
                    ipAddress: ipAddress,
                    userAgent: userAgent
                );

                throw new Exception("User is disabled");
            }

            //  PASSWORD WRONG
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                await _audit.LogAsync(
                    user.Id,
                    "Auth.Login.Failed.InvalidPassword",
                    ipAddress: ipAddress,
                    userAgent: userAgent
                );

                throw new Exception("Invalid credentials");
            }

            //  SUCCESS
            var accessToken = GenerateJwtToken(user);
            var refreshTokenValue = GenerateRefreshToken();

            var refreshDays = dto.RememberMe
                ? int.Parse(_config["Jwt:RefreshTokenDaysRememberMe"])
                : int.Parse(_config["Jwt:RefreshTokenDays"]);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
            };

            await _refreshTokens.AddAsync(refreshToken);
            await _refreshTokens.SaveChangesAsync();

            await _audit.LogAsync(
                user.Id,
                "Auth.Login.Success",
                ipAddress: ipAddress,
                userAgent: userAgent
            );

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }


        public async Task<LoginResponseDto> RefreshAsync(
            string refreshToken,
            string? ip,
            string? ua)
        {
            var token = await _refreshTokens.GetAsync(refreshToken)
                ?? throw new Exception("Invalid refresh token");

            if (token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Refresh token expired");

            token.IsRevoked = true;

            var user = token.User;

            var newAccess = GenerateJwtToken(user);
            var newRefresh = GenerateRefreshToken();

            await _refreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = newRefresh,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _refreshTokens.SaveChangesAsync();

        

            return new LoginResponseDto
            {
                AccessToken = newAccess,
                RefreshToken = newRefresh
            };
        }
        public async Task LogoutAsync(
            string refreshToken,
            Guid userId,
            string? ip,
            string? ua)
        {
            var token = await _refreshTokens.GetAsync(refreshToken);
            if (token == null) return;

            token.IsRevoked = true;
            await _refreshTokens.SaveChangesAsync();

        }
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("name", user.FullName)
    };

            if (user.UserRoles != null && user.UserRoles.Any())
            {
                var roleNames = user.UserRoles
                    .Select(ur => ur.Role.Name)
                    .ToList();

                foreach (var role in roleNames)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                claims.Add(new Claim("roles", string.Join(",", roleNames)));

                //  
                var effectivePermissions =
                    _permissions.GetEffectivePermissionsAsync(user.Id)
                                .GetAwaiter()
                                .GetResult();
                if (user.UserRoles.Any(r => r.Role.Name == "Admin"))
                {
                    effectivePermissions = Enum
                        .GetValues<Permission>()
                        .ToHashSet();
                }

                claims.Add(new Claim(
                    "permissions",
                    string.Join(",", effectivePermissions.Select(p => p.ToString()))
                ));
            }

            var accessMinutes =
                int.Parse(_config["Jwt:AccessTokenMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string GenerateRefreshToken()
        {
            var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

      


        public async Task UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _users.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.IsActive= dto.IsActive;
            if(!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                await _refreshTokens.RevokeAllForUserAsync(user.Id);
            }

            await _users.SaveChangesAsync();
        }



        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _users.GetByIdAsync(id)
                       ?? throw new Exception("User not found");

            // fshi edhe rolet e userit
            if (user.UserRoles != null && user.UserRoles.Any())
            {
                await _userRoles.RemoveRangeAsync(user.UserRoles);
            }

            await _users.DeleteAsync(user);
            await _users.SaveChangesAsync();
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _users.GetByEmailAsync(email);
            if (user == null)
                return; // silent fail (security best practice)

            var rawToken = GenerateSecureToken();
            var hashedToken = TokenHasher.Hash(rawToken);

            var token = new PasswordResetToken
            {
                UserId = user.Id,
                Token = hashedToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            await _passwordResetTokens.AddAsync(token);
            await _passwordResetTokens.SaveChangesAsync();

            var encodedToken = Uri.EscapeDataString(rawToken);

            var link =
                $"{_config["Frontend:BaseUrl"]}/auth/reset-password?token={encodedToken}";

            await _email.SendAsync(
                user.Email,
                "Reset your password",
                $"Click the link to reset your password:\n{link}"
            );
        }

        public async Task ResetPasswordAsync(
            string tokenValue,
            string newPassword,
            string? ip,
            string? ua)
        {
            var decodedToken = Uri.UnescapeDataString(tokenValue);
            var hashedToken = TokenHasher.Hash(decodedToken);

            var token = await _passwordResetTokens.GetValidAsync(hashedToken)
                ?? throw new Exception("Invalid or expired token");

            var user = token.User;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            token.IsUsed = true;

            await _refreshTokens.RevokeAllForUserAsync(user.Id);

            await _users.SaveChangesAsync();
            await _passwordResetTokens.SaveChangesAsync();

         
        }
        private static string GenerateSecureToken()
        {
            var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(64);
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
}
