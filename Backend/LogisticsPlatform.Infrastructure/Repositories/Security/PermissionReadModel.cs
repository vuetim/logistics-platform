using LogisticsPlatform.Domain.Security;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Security;

public class PermissionReadModel : IPermissionReadModel
{
    private readonly AppDbContext _db;

    public PermissionReadModel(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Guid>> GetUserRoleIdsAsync(Guid userId)
        => await _db.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();

    public async Task<List<(Permission, bool)>> GetRolePermissionsAsync(IEnumerable<Guid> roleIds)
        => await _db.RolePermissions
            .Where(x => roleIds.Contains(x.RoleId))
            .Select(x => new ValueTuple<Permission, bool>(
                x.Permission,
                x.IsAllowed))
            .ToListAsync();

    public async Task<List<(Permission, bool?)>> GetUserOverridesAsync(Guid userId)
        => await _db.UserPermissions
            .Where(x => x.UserId == userId)
            .Select(x => new ValueTuple<Permission, bool?>(
                x.Permission,
                x.IsAllowed))
            .ToListAsync();
    public async Task<bool> IsUserInRoleAsync(IEnumerable<Guid> roleIds, string roleName)
    {
        return await _db.Roles
            .AnyAsync(r => roleIds.Contains(r.Id) && r.Name == roleName);
    }
}
