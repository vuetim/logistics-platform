using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Security;

public interface IJwtProvider
{
    Task<string> GenerateAccessTokenAsync(User user, CancellationToken ct = default);
}
