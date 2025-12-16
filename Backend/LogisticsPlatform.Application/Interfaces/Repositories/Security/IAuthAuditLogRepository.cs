using LogisticsPlatform.Domain.Entities.Security;


namespace LogisticsPlatform.Application.Interfaces.Repositories.Security
{
    public interface IAuthAuditLogRepository
    {
        Task AddAsync(AuthAuditLog log);
        Task SaveChangesAsync();
    }
}
