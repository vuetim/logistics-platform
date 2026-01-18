using LogisticsPlatform.Domain.Security;

namespace LogisticsPlatform.Domain.Entities.Security;

public class UserPermission
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Permission Permission { get; set; }

    // true  = ALLOW override
    // false = DENY override
    public bool? IsAllowed { get; set; }
}
