using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _ctx;

    public RefreshTokenRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(RefreshToken token)
        => await _ctx.RefreshTokens.AddAsync(token);

    public Task<RefreshToken?> GetAsync(string token)
        => _ctx.RefreshTokens
            .Include(t => t.User)
            .ThenInclude(u => u.UserRoles!)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(t => t.Token == token);
    public async Task RevokeAllForUserAsync(Guid userId)
    {
        var tokens = await _ctx.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
            token.IsRevoked = true;
    }
    public Task SaveChangesAsync()
        => _ctx.SaveChangesAsync();
}
