using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderCancellationPolicy
{
    void EnsureCanCancel(IEnumerable<Load> linkedLoads);
}

