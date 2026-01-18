using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Application.DTOs.Auth;

public class UserPermissionStateDto
{
    public Permission Permission { get; set; }
    public bool? IsAllowed { get; set; }
    // true = allow
    // false = deny
    // null = inherited (from role)
}
