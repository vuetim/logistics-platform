using LogisticsPlatform.Domain.Entities;

public interface ILoadCarrierAssignmentRepository
{
    Task AddAsync(LoadCarrierAssignment assignment);
    Task UpdateAsync(LoadCarrierAssignment assignment);

    Task<LoadCarrierAssignment?> GetByIdAsync(Guid id);
    Task<LoadCarrierAssignment?> GetByTenderTokenAsync(string token);

    Task<IEnumerable<LoadCarrierAssignment>> GetByLoadIdAsync(Guid loadId);

    Task<LoadCarrierAssignment?> GetActiveByLoadAsync(Guid loadId);
    Task<IReadOnlyList<LoadCarrierAssignment>> GetOpenTenderedAsync();

    Task SaveChangesAsync();
}
