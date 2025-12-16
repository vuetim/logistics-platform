using LogisticsPlatform.Domain.Entities.Financial;

public interface ICarrierSettlementRepository
{
    Task<CarrierSettlement?> GetByLoadIdAsync(Guid loadId);
    Task<CarrierSettlement?> GetByIdAsync(Guid id);

    Task AddAsync(CarrierSettlement settlement);
    Task SaveChangesAsync();
}
