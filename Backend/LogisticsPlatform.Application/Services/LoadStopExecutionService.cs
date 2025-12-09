using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
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
        private readonly IActivityLogService _activityLog;

        public LoadStopExecutionService(
            ILoadStopRepository stops,
            ILoadRepository loads,
            ILoadStatusCalculatorService calculator,
            IActivityLogService activityLog)
        {
            _stops = stops;
            _loads = loads;
            _calculator = calculator;
            _activityLog = activityLog;
        }

        // =============================
        // PICKUP / DELIVERY WORKFLOW
        // =============================

        public async Task MarkEnRouteAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            var oldStatus = stop.Status;
            stop.Status = StopStatus.EnRoute;

            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked EnRoute");
        }

        public async Task MarkArrivedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            var oldStatus = stop.Status;
            stop.ActualArrival ??= DateTime.UtcNow;
            stop.Status = StopStatus.Arrived;

            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Arrived");
        }

        public async Task MarkLoadedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.StopType != StopType.Pickup)
                throw new BusinessRuleException("Only pickup stops can be marked as Loaded.");

            var oldStatus = stop.Status;

            stop.ActualArrival ??= DateTime.UtcNow;
            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Loaded;

            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Loaded");
        }

        public async Task MarkUnloadedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.StopType != StopType.Delivery)
                throw new BusinessRuleException("Only delivery stops can be marked as Unloaded.");

            var oldStatus = stop.Status;

            stop.ActualArrival ??= DateTime.UtcNow;
            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Completed;

            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Unloaded");
        }

        // =============================
        // CORE HELPER (✅ SINGLE SOURCE)
        // =============================
        private async Task UpdateLoadStatusAndLogAsync(
            LoadStop stop,
            StopStatus oldStopStatus,
            Guid userId,
            string action)
        {
            var load = stop.Load;

            var oldLoadStatus = load.Status;
            load.Status = _calculator.Calculate(load);

            await _stops.UpdateAsync(stop);
            await _loads.SaveChangesAsync();

            // 🔹 Stop activity
            await _activityLog.LogAsync(new ActivityLogEntry
            {
                EntityType = ActivityEntityType.Load.ToString(),
                EntityId = load.Id,
                ActivityType = ActivityType.Load_StopStatusChanged,
                PerformedByUserId = userId,
                Summary =
                    $"Stop #{stop.Sequence} ({stop.StopType}) {action}: {oldStopStatus} → {stop.Status}",
                Details = null
            });

            // 🔹 Load activity ONLY if changed
            if (oldLoadStatus != load.Status)
            {
                await _activityLog.LogAsync(new ActivityLogEntry
                {
                    EntityType = ActivityEntityType.Load.ToString(),
                    EntityId = load.Id,
                    ActivityType = ActivityType.Load_StatusChanged,
                    PerformedByUserId = userId,
                    Summary =
                        $"Load status changed from {oldLoadStatus} → {load.Status}",
                    Details = null
                });
            }
        }

        // =============================
        // PRIVATE GUARDS
        // =============================
        private async Task<LoadStop> GetStopOrThrow(Guid stopId)
            => await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

        private static void EnsureNotCompleted(LoadStop stop)
        {
            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");
        }
    }
}
