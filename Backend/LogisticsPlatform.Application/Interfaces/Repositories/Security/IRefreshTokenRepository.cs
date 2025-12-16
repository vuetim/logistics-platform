using LogisticsPlatform.Domain.Entities;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetAsync(string token);

    Task RevokeAllForUserAsync(Guid userId);

    Task SaveChangesAsync();
}
