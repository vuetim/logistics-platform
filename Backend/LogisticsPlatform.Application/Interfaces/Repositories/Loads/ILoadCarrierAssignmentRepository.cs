using LogisticsPlatform.Domain.Entities;

public interface ILoadCarrierAssignmentRepository
{
    Task AddAsync(LoadCarrierAssignment assignment);
    Task UpdateAsync(LoadCarrierAssignment assignment);

    Task<LoadCarrierAssignment?> GetByIdAsync(Guid id);

    Task<IEnumerable<LoadCarrierAssignment>> GetByLoadIdAsync(Guid loadId);

    Task<LoadCarrierAssignment?> GetActiveByLoadAsync(Guid loadId);

    Task SaveChangesAsync();
}
