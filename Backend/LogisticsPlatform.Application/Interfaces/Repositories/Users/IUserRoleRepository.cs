using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole entity);
        Task RemoveRangeAsync(IEnumerable<UserRole> roles);

    }
}
