using LogisticsPlatform.Application.Interfaces;
using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;

        public AuthService(IUserRepository users)
        {
            _users = users;
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
    }
}
