using LogisticsPlatform.Domain.Security;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Security;

namespace LogisticsPlatform.Application.Security;

public class PermissionService : IPermissionService
{
    private readonly IPermissionReadModel _read;

    public PermissionService(IPermissionReadModel read)
    {
        _read = read;
    }

    public async Task<HashSet<Permission>> GetEffectivePermissionsAsync(Guid userId)
    {
        var roleIds = await _read.GetUserRoleIdsAsync(userId);
        var rolePerms = await _read.GetRolePermissionsAsync(roleIds);

        var effective = new Dictionary<Permission, bool>();

        foreach (var rp in rolePerms)
        {
            effective[rp.Permission] =
                effective.TryGetValue(rp.Permission, out var prev)
                    ? prev || rp.IsAllowed
                    : rp.IsAllowed;
        }

        var overrides = await _read.GetUserOverridesAsync(userId);

        foreach (var up in overrides.Where(x => x.IsAllowed != null))
            effective[up.Permission] = up.IsAllowed!.Value;

        return effective
            .Where(x => x.Value)
            .Select(x => x.Key)
            .ToHashSet();
    }

    public async Task<bool> HasPermissionAsync(Guid userId, Permission permission)
    {
        //  ADMIN = ALL (hard rule)
        var roleIds = await _read.GetUserRoleIdsAsync(userId);
        var isAdmin = await _read.IsUserInRoleAsync(roleIds, "Admin");

        if (isAdmin)
            return true;

        var perms = await GetEffectivePermissionsAsync(userId);
        return perms.Contains(permission);
    }

  
}
