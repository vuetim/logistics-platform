using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Domain.Entities.Security;
using LogisticsPlatform.Infrastructure.Persistence;

namespace LogisticsPlatform.Infrastructure.Repositories.Security;

public class AuthAuditLogRepository : IAuthAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuthAuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuthAuditLog log)
        => await _context.AuthAuditLogs.AddAsync(log);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
