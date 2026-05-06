using LogisticsPlatform.Domain.Entities.Financial;

public interface ICarrierSettlementRepository
{
    Task<CarrierSettlement?> GetByLoadIdAsync(Guid loadId);
    Task<CarrierSettlement?> GetByIdAsync(Guid id);
    Task<List<CarrierSettlement>> ListAsync();
    Task DeleteLineItemsBySettlementIdAsync(Guid settlementId);
    Task AddLineItemsAsync(IEnumerable<CarrierSettlementLineItem> lineItems);

    Task AddAsync(CarrierSettlement settlement);
    Task SaveChangesAsync();
}
