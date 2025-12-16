using System;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadStopExecutionService
    {
        Task MarkEnRouteAsync(Guid stopId, Guid userId);
        Task MarkArrivedAsync(Guid stopId, Guid userId);
        Task MarkLoadedAsync(Guid stopId, Guid userId);
        Task MarkUnloadedAsync(Guid stopId, Guid userId);
    }
}
