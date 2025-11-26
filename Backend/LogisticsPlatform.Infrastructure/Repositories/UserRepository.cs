using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public Task<User?> GetByEmailAsync(string email) =>
            _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

        public Task<User?> GetByIdAsync(Guid id) =>
            _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();

        public Task<List<User>> GetAllAsync() =>
    _context.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .ToListAsync();

        public Task<List<User>> GetByRoleAsync(string roleName) =>
            _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .ToListAsync();
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
        }

    }
}
