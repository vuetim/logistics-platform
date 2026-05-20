using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public sealed class LoadCostSnapshot
{
    public LoadCostSnapshot(decimal customerRate, decimal accessorials, LoadCost? cost)
    {
        CustomerRate = customerRate;
        Accessorials = accessorials;
        Cost = cost;
    }

    public decimal CustomerRate { get; }
    public decimal Accessorials { get; }
    public LoadCost? Cost { get; }
}

