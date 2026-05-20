using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class LoadExceptionRepository : ILoadExceptionRepository
{
    private readonly AppDbContext _ctx;

    public LoadExceptionRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IReadOnlyList<LoadException>> GetByLoadAsync(Guid loadId)
    {
        return await _ctx.LoadExceptions
            .AsNoTracking()
            .Where(x => x.LoadId == loadId)
            .OrderByDescending(x => x.OccurredAt)
            .ToListAsync();
    }

    public async Task<LoadException?> GetByIdForLoadAsync(Guid loadId, Guid exceptionId)
    {
        return await _ctx.LoadExceptions
            .FirstOrDefaultAsync(x => x.Id == exceptionId && x.LoadId == loadId);
    }

    public async Task<bool> ExistsOpenAsync(Guid loadId, Guid? stopId, string exceptionKey, string reasonKey)
    {
        return await _ctx.LoadExceptions.AnyAsync(x =>
            x.LoadId == loadId &&
            x.LoadStopId == stopId &&
            x.ExceptionKey == exceptionKey &&
            x.ReasonKey == reasonKey &&
            x.Status != LoadExceptionStatus.Resolved &&
            x.Status != LoadExceptionStatus.Rejected &&
            x.Status != LoadExceptionStatus.Cancelled);
    }

    public async Task AddAsync(LoadException exception)
    {
        await _ctx.LoadExceptions.AddAsync(exception);
        await _ctx.SaveChangesAsync();
    }

    public Task UpdateAsync(LoadException exception)
    {
        _ctx.LoadExceptions.Update(exception);
        return _ctx.SaveChangesAsync();
    }
}
