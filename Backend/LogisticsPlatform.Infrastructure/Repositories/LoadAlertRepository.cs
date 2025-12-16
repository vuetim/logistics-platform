using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadAlertRepository : ILoadAlertRepository
{
    private readonly AppDbContext _ctx;

    public LoadAlertRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> ExistsAsync(
        Guid loadId,
        Guid? stopId,
        AlertType type,
        AlertSeverity severity)
    {
        return await _ctx.LoadAlerts.AnyAsync(a =>
            a.LoadId == loadId &&
            a.LoadStopId == stopId &&
            a.Type == type &&
            a.Severity == severity);
    }

    public async Task AddAsync(LoadAlert alert)
    {
        await _ctx.LoadAlerts.AddAsync(alert);
        await _ctx.SaveChangesAsync();
    }
}
