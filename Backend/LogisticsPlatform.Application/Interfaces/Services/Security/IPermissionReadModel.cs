using LogisticsPlatform.Domain.Security;

public interface IPermissionReadModel
{
    Task<List<Guid>> GetUserRoleIdsAsync(Guid userId);

    Task<List<(Permission Permission, bool IsAllowed)>>
        GetRolePermissionsAsync(IEnumerable<Guid> roleIds);

    Task<List<(Permission Permission, bool? IsAllowed)>>
        GetUserOverridesAsync(Guid userId);
    Task<bool> IsUserInRoleAsync(IEnumerable<Guid> roleIds, string roleName);

}
