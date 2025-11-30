using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Authorization;

public class AuthorizationService : IAuthorizationService
{
    public bool HasPermission(
        User user,
        Permission permission,
        object? resource = null)
    {
        var roles = user.UserRoles.Select(r => r.Role.Name);

        foreach (var role in roles)
        {
            if (RolePermissions.Has(role, permission))
                return true;
        }

        return false;
    }
}
