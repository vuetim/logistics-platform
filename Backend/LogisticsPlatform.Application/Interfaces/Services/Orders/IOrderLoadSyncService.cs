using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderLoadSyncService
{
    Task SyncFromLoadAsync(Load load);
    Task SyncByOrderIdAsync(Guid orderId);
}

