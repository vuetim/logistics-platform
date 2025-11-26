using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRole entity)
        {
            await _context.UserRoles.AddAsync(entity);
        }
        public async Task RemoveRangeAsync(IEnumerable<UserRole> roles)
        {
            _context.UserRoles.RemoveRange(roles);
        }

    }
}
