using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface ILoadCostSnapshotBuilder
{
    LoadCostSnapshot Build(Order order);
}

