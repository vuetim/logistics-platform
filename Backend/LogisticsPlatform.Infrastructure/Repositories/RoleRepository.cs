using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) => _context = context;

        public Task<Role?> GetByNameAsync(string name) =>
            _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public Task<List<Role>> GetAllAsync() =>
            _context.Roles.ToListAsync();
    }
}
