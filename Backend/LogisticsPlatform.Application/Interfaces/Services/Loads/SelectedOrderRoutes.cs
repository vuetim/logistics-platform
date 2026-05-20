using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public sealed class SelectedOrderRoutes
{
    public SelectedOrderRoutes(IReadOnlyList<OrderRoute> routes)
    {
        Routes = routes;
    }

    public IReadOnlyList<OrderRoute> Routes { get; }
    public OrderRoute First => Routes[0];
    public OrderRoute Last => Routes[^1];
}

