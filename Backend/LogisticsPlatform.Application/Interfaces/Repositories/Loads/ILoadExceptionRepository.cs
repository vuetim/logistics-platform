using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public interface ILoadExceptionRepository
{
    Task<IReadOnlyList<LoadException>> GetByLoadAsync(Guid loadId);
    Task<LoadException?> GetByIdForLoadAsync(Guid loadId, Guid exceptionId);
    Task<bool> ExistsOpenAsync(Guid loadId, Guid? stopId, string exceptionKey, string reasonKey);
    Task AddAsync(LoadException exception);
    Task UpdateAsync(LoadException exception);
}
