using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public sealed class OrderToLoadSnapshot
{
    public OrderToLoadSnapshot(Load load, IReadOnlyList<LoadStop> stops, LoadOrder loadOrder)
    {
        Load = load;
        Stops = stops;
        LoadOrder = loadOrder;
    }

    public Load Load { get; }
    public IReadOnlyList<LoadStop> Stops { get; }
    public LoadOrder LoadOrder { get; }
}

