using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Persistence;

public static class RolePermissionDefaultsSeeder
{
    private static readonly string[] SystemRoles =
    {
        RoleNames.Admin,
        RoleNames.Broker,
        RoleNames.Sales,
        RoleNames.Operator,
        RoleNames.Dispatcher,
        RoleNames.Accounting
    };

    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        var roles = await db.Roles
            .Where(role => SystemRoles.Contains(role.Name))
            .ToListAsync(ct);

        var existingRoleNames = roles
            .Select(role => role.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingRoles = SystemRoles
            .Where(roleName => !existingRoleNames.Contains(roleName))
            .Select(roleName => new Role { Name = roleName })
            .ToList();

        if (missingRoles.Count > 0)
        {
            await db.Roles.AddRangeAsync(missingRoles, ct);
            await db.SaveChangesAsync(ct);
            roles.AddRange(missingRoles);
        }

        foreach (var role in roles)
        {
            var existing = await db.RolePermissions
                .Where(permission => permission.RoleId == role.Id)
                .ToListAsync(ct);
            var defaultPermissions = RolePermissions.Get(role.Name).ToHashSet();

            var stale = existing
                .Where(permission => !defaultPermissions.Contains(permission.Permission));

            db.RolePermissions.RemoveRange(stale);

            var existingPermissions = existing
                .Select(permission => permission.Permission)
                .ToHashSet();

            var missing = defaultPermissions
                .Where(permission => !existingPermissions.Contains(permission))
                .Select(permission => new RolePermission
                {
                    RoleId = role.Id,
                    Permission = permission,
                    IsAllowed = true
                });

            await db.RolePermissions.AddRangeAsync(missing, ct);
        }

        await db.SaveChangesAsync(ct);
    }
}
