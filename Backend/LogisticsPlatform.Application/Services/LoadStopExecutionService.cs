using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;

namespace LogisticsPlatform.Application.Services
{
    public class LoadStopExecutionService : ILoadStopExecutionService
    {
        private readonly ILoadStopRepository _stops;
        private readonly ILoadRepository _loads;
        private readonly ILoadStatusCalculatorService _calculator;

        public LoadStopExecutionService(
            ILoadStopRepository stops,
            ILoadRepository loads,
            ILoadStatusCalculatorService calculator)
        {
            _stops = stops;
            _loads = loads;
            _calculator = calculator;
        }

        public async Task MarkEnRouteAsync(Guid stopId, Guid userId)
        {
            var stop = await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");

            stop.Status = StopStatus.EnRoute;

            await UpdateLoadStatusAsync(stop);
        }

        public async Task MarkArrivedAsync(Guid stopId, Guid userId)
        {
            var stop = await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");

            stop.ActualArrival ??= DateTime.UtcNow;
            stop.Status = StopStatus.Arrived;

            await UpdateLoadStatusAsync(stop);
        }

        public async Task MarkLoadedAsync(Guid stopId, Guid userId)
        {
            var stop = await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

            if (stop.StopType != StopType.Pickup)
                throw new BusinessRuleException("Only pickup stops can be marked as loaded.");

            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");

            stop.ActualArrival ??= DateTime.UtcNow;
            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Loaded;

            await UpdateLoadStatusAsync(stop);
        }

        public async Task MarkUnloadedAsync(Guid stopId, Guid userId)
        {
            var stop = await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

            if (stop.StopType != StopType.Delivery)
                throw new BusinessRuleException("Only delivery stops can be marked as unloaded.");

            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");

            stop.ActualArrival ??= DateTime.UtcNow;
            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Completed;

            await UpdateLoadStatusAsync(stop);
        }

        // =============================
        // HELPER: Recalculate Load.Status
        // =============================
        private async Task UpdateLoadStatusAsync(LoadStop stop)
        {
            // EF trackon load dhe stops në të njëjtin context
            var load = stop.Load;

            load.Status = _calculator.Calculate(load);

            await _stops.UpdateAsync(stop);
            await _loads.SaveChangesAsync();
        }
    }
}
