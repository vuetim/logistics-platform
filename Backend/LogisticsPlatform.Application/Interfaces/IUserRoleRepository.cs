using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole entity);
    }
}
