using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
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

        public AuthService(
    IUserRepository users,
    IRoleRepository roles,
    IUserRoleRepository userRoles,
    IConfiguration config)
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
            var user = await _users.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid)
                throw new Exception("Invalid credentials");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ⭐ Këtu është ndryshimi kryesor -> tani është List<Claim>
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.FullName)
            };

            // ⭐ Shto Roles nese ekzistojnë
            if (user.UserRoles != null && user.UserRoles.Any())
            {
                var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
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
            // 1. Merr User-in
            var user = await _users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");

            // 2. Merr Role-in
            var role = await _roles.GetByNameAsync(dto.RoleName);
            if (role == null)
                throw new Exception("Role not found");

            // 3. Kontrollo nëse useri e ka veç rolin
            if (user.UserRoles != null && user.UserRoles.Any(ur => ur.RoleId == role.Id))
                throw new Exception("User already has this role");

            // 4. Shto UserRole
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRoles.AddAsync(userRole);

            // 5. Ruaj në database
            await _users.SaveChangesAsync();
        }

    }
}
