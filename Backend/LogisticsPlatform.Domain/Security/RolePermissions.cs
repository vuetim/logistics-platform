using LogisticsPlatform.Domain.Constants;

namespace LogisticsPlatform.Domain.Security;

public static class RolePermissions
{
    private static readonly Dictionary<string, HashSet<Permission>> Map =
        new(StringComparer.OrdinalIgnoreCase) //   case-insensitive
        {
            [RoleNames.Admin] = Enum
                .GetValues<Permission>()
                .ToHashSet(),

            [RoleNames.Broker] = new()
            {
                Permission.Load_View,
                Permission.Load_Create,
                Permission.Load_Update,
                Permission.Load_Operational_Update,
                Permission.Load_ChangeStatus,
                Permission.Load_Archive,
                Permission.Load_Dispatch,
                Permission.Load_Tender,
                Permission.Load_CreateFromOrder,
                Permission.Load_Tracking_View,
                Permission.Customer_View,
                Permission.Customer_Create,
                Permission.Customer_Update,
                Permission.OrderCost_View,
                Permission.OrderCost_Update,
                Permission.LoadCost_View,
                Permission.LoadCost_Update,
                Permission.Financial_View,
                Permission.Financial_Invoice_UpdateStatus,
                Permission.Financial_Invoice_RecordPayment,
                Permission.Financial_Settlement_UpdateStatus,
                Permission.Financial_Settlement_RecordPayment,


                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,
                Permission.LoadNote_Create_Public,

                Permission.LoadDocument_View,
                Permission.LoadDocument_Upload,

                  Permission.User_View_Self,
            },

            [RoleNames.Sales] = new()
            {
                Permission.Load_View,
                Permission.Load_Create,
                Permission.Load_Update,
                Permission.Load_Operational_Update,
                Permission.Load_ChangeStatus,
                Permission.Load_CreateFromOrder,
                Permission.Load_Tracking_View,
                Permission.Customer_View,
                Permission.Customer_Create,
                Permission.Customer_Update,
                Permission.OrderCost_View,
                Permission.OrderCost_Update,
                Permission.LoadCost_View,
                Permission.LoadCost_Update,
                Permission.Financial_View,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,
                Permission.LoadNote_Create_Public,

                Permission.LoadDocument_View,
                Permission.LoadDocument_Upload,

                Permission.User_View_Self,
            },

            [RoleNames.Operator] = new()
            {
                Permission.Load_View,
                Permission.Load_Operational_Update,
                Permission.Load_ChangeStatus,
                Permission.Load_Tracking_View,
                Permission.Load_Tracking_Update,
                Permission.LoadCost_View,
                Permission.OrderCost_View,
                Permission.Financial_View,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,

                Permission.LoadDocument_View,

                Permission.User_View_Self,
                Permission.Customer_View,
                Permission.LoadException_View,
                Permission.LoadException_Create,
                Permission.LoadException_Update,
                Permission.LoadStopService_View,
                Permission.LoadStopService_Create,

            },

            [RoleNames.Dispatcher] = new()
            {
                Permission.Load_View,
                Permission.Load_Operational_Update,
                Permission.Load_ChangeStatus,
                Permission.Load_Dispatch,
                Permission.Load_Tender,
                Permission.Load_Tracking_View,
                Permission.Load_Tracking_Update,
                Permission.LoadCost_View,
                Permission.OrderCost_View,

                Permission.LoadNote_View,
                Permission.LoadNote_Create_Internal,
                Permission.Customer_View,
                Permission.User_View_Self,
                Permission.CarrierOffer_View,
                Permission.CarrierOffer_Create,
                Permission.CarrierOffer_Accept,
                Permission.CarrierOffer_Reject,
                Permission.LoadException_View,
                Permission.LoadException_Create,
                Permission.LoadException_Update,
                Permission.LoadStopService_View,
                Permission.LoadStopService_Create,


            },

            [RoleNames.Accounting] = new()
            {
                Permission.Load_View,
                Permission.Customer_View,
                Permission.OrderCost_View,
                Permission.LoadCost_View,
                Permission.Financial_View,
                Permission.Financial_Invoice_UpdateStatus,
                Permission.Financial_Invoice_RecordPayment,
                Permission.Financial_Settlement_UpdateStatus,
                Permission.Financial_Settlement_RecordPayment,
                Permission.User_View_Self
            }
        };

    //  used in HasPermission (backend checks)
    public static bool Has(string role, Permission permission)
        => Map.TryGetValue(role, out var perms) && perms.Contains(permission);

    //  used in JWT generation
    public static IReadOnlyCollection<Permission> Get(string role)
        => Map.TryGetValue(role, out var perms)
            ? perms
            : Array.Empty<Permission>();

    
    public static bool TryGet(string role, out HashSet<Permission> permissions)
        => Map.TryGetValue(role, out permissions!);
}
