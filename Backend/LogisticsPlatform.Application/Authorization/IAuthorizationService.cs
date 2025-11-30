using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Authorization;

public interface IAuthorizationService
{
    bool HasPermission(
        User user,
        Permission permission,
        object? resource = null);
}
