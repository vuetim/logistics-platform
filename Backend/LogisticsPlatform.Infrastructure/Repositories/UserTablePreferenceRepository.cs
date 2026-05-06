using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Domain.Entities.Security;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class UserTablePreferenceRepository : IUserTablePreferenceRepository
    {
        private readonly AppDbContext _context;

        public UserTablePreferenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserTablePreference?> GetAsync(Guid userId, string tableKey)
        {
            return await _context.UserTablePreferences
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TableKey == tableKey);
        }

        public async Task AddAsync(UserTablePreference preference)
        {
            await _context.UserTablePreferences.AddAsync(preference);
        }

        public void Update(UserTablePreference preference)
        {
            _context.UserTablePreferences.Update(preference);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
