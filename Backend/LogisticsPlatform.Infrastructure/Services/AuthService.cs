using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticsPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IUserRoleRepository _userRoles;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IRoleRepository roles, IUserRoleRepository userRoles, IConfiguration config)
        {
            _users = users;
            _roles = roles;
            _userRoles = userRoles;
            _config = config;
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

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.Email)
                       ?? throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return GenerateJwtToken(user);
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

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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


    }
}
