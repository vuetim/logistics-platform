using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface IOrderToLoadSnapshotBuilder
{
    OrderToLoadSnapshot Build(Order order, CreateLoadFromOrderDto dto, Guid userId, SelectedOrderRoutes routes);
}

