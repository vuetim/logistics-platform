using LogisticsPlatform.Application.Interfaces.Repositories.Notifications;
using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Notifications;

public class NotificationRecipientRepository : INotificationRecipientRepository
{
    private static readonly string[] InternalRoles =
    {
        RoleNames.Admin,
        RoleNames.Broker,
        RoleNames.Sales,
        RoleNames.Operator,
        RoleNames.Dispatcher,
        RoleNames.Accounting
    };

    private readonly AppDbContext _db;

    public NotificationRecipientRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Guid>> GetInternalRecipientIdsAsync(Guid actorUserId)
        => _db.Users
            .AsNoTracking()
            .Where(user =>
                user.IsActive &&
                user.Id != actorUserId &&
                user.UserRoles != null &&
                user.UserRoles.Any(userRole => InternalRoles.Contains(userRole.Role.Name)))
            .Select(user => user.Id)
            .ToListAsync();
}
