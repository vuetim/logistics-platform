using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface IOrderToLoadRouteSelector
{
    SelectedOrderRoutes Select(Order order);
}

