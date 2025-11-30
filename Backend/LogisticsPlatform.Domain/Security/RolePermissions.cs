using LogisticsPlatform.Domain.Constants;

namespace LogisticsPlatform.Domain.Security;

public static class RolePermissions
{
    private static readonly Dictionary<string, HashSet<Permission>> Map =
        new()
        {
            [RoleNames.Admin] = Enum.GetValues<Permission>().ToHashSet(),

            [RoleNames.Broker] = new()
            {
                Permission.Load_View,
                Permission.Load_Create,
                Permission.Load_Update,
                Permission.Load_ChangeStatus,
                Permission.Load_Archive,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,
                Permission.LoadNote_Create_Public,

                Permission.LoadDocument_View,
                Permission.LoadDocument_Upload
            },

            [RoleNames.Operator] = new()
            {
                Permission.Load_View,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,

                Permission.LoadDocument_View
            },

            [RoleNames.Dispatcher] = new()
            {
                Permission.Load_View,
                Permission.Load_ChangeStatus,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal
            }
        };

    public static bool Has(string role, Permission permission)
        => Map.TryGetValue(role, out var perms) && perms.Contains(permission);
}
