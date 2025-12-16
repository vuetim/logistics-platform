using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Auth;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthService(IUserRepository users, IRoleRepository roles, IUserRoleRepository userRoles, IConfiguration config,
            IRefreshTokenRepository refreshTokens, IEmailService email, IPasswordResetTokenRepository passwordResetTokens)
        {
            _users = users;
            _roles = roles;
            _userRoles = userRoles;
            _refreshTokens = refreshTokens;
            _config = config;
            _email = email;
            _passwordResetTokens = passwordResetTokens;
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _users.AddAsync(user);
            await _users.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.Email)
                       ?? throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var accessToken = GenerateJwtToken(user);
            var refreshTokenValue = GenerateRefreshToken();

            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]);

            var days = dto.RememberMe
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

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }

        public async Task<LoginResponseDto> RefreshAsync(string refreshToken)
        {
            var token = await _refreshTokens.GetAsync(refreshToken)
                ?? throw new Exception("Invalid refresh token");

            if (token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Refresh token expired");

            token.IsRevoked = true; // ROTATION

            var user = token.User;

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshValue = GenerateRefreshToken();

            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]);

            var newRefresh = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshValue,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
            };


            await _refreshTokens.AddAsync(newRefresh);
            await _refreshTokens.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshValue
            };
        }
        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _refreshTokens.GetAsync(refreshToken);
            if (token == null)
                return; // silent logout (OK practice)

            token.IsRevoked = true;
            await _refreshTokens.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.FullName)
            };

            if (user.UserRoles != null && user.UserRoles.Any())
            {
                var roleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                foreach (var role in roleNames)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                claims.Add(new Claim("roles", string.Join(",", roleNames)));
            }

            var accessMinutes = int.Parse(_config["Jwt:AccessTokenMinutes"]);

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

        public async Task AssignRoleAsync(AssignRoleDto dto)
        {
            var user = await _users.GetByIdAsync(dto.UserId)
                       ?? throw new Exception("User not found");

            var role = await _roles.GetByNameAsync(dto.RoleName)
                       ?? throw new Exception("Role not found");

            // Remove existing roles 
            if (user.UserRoles != null && user.UserRoles.Any())
            {
                var existingRoles = user.UserRoles.ToList();
                await _userRoles.RemoveRangeAsync(existingRoles);
            }

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRoles.AddAsync(userRole);
            await _users.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _users.GetAllAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Roles = u.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new()
            }).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _users.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new()
            };
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _users.GetByRoleAsync(roleName);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Roles = u.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new()
            }).ToList();
        }
        public async Task UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _users.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            user.FullName = dto.FullName;
            user.Email = dto.Email;

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
        public async Task<Guid?> ForgotPasswordAsync(string email)
        {
            var user = await _users.GetByEmailAsync(email);
            if (user == null)
                return null;

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
                $"{_config["Frontend:BaseUrl"]}/reset-password?token={encodedToken}";

            await _email.SendAsync(
                user.Email,
                "Reset your password",
                $"Click the link to reset your password:\n{link}"
            );

            return user.Id;
        }

        public async Task<Guid> ResetPasswordAsync(string tokenValue, string newPassword)
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

            return user.Id;
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
