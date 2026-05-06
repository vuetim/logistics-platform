using LogisticsPlatform.Domain.Entities.Security;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Users
{
    public interface IUserTablePreferenceRepository
    {
        Task<UserTablePreference?> GetAsync(Guid userId, string tableKey);
        Task AddAsync(UserTablePreference preference);
        void Update(UserTablePreference preference);
        Task SaveChangesAsync();
    }
}
