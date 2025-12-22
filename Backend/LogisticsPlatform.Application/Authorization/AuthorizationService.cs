using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.Authorization;

public class AuthorizationService : IAuthorizationService
{
    public bool HasPermission(User user, Permission permission, object? resource = null)
    {
        var roles = user.UserRoles.Select(r => r.Role.Name);

        //  Role-based 
        foreach (var role in roles)
        {
            if (RolePermissions.Has(role, permission))
            {
                //  USER resource rule (vetvetja)
                if (resource is User targetUser)
                {
                    if (permission == Permission.User_View_Self ||
                        permission == Permission.User_Update)
                    {
                        return user.Id == targetUser.Id
                            || roles.Contains(RoleNames.Admin);
                    }
                }

                return true;
            }
        }

        return false;
    }

}
