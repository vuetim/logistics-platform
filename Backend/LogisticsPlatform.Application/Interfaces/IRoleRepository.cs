using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
