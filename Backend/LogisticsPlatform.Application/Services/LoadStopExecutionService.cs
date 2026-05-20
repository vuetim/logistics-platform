using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
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
        private readonly ICarrierPerformanceService _carrierPerformanceService;
        private readonly IEtaPredictionService _etaPredictionService;
        private readonly ILoadAlertService _alertService;
        private readonly IDelayFaultAttributionService _delayFaultAttributionService;
        private readonly IOrderLoadSyncService _orderLoadSyncService;
        private readonly INotificationService _notifications;

        public LoadStopExecutionService(
            ILoadStopRepository stops,
            ILoadRepository loads,
            ILoadStatusCalculatorService calculator,
            IActivityLogService activityLog,
            ICarrierPerformanceService carrierPerformanceService,
            IEtaPredictionService etaPredictionService,
            ILoadAlertService alertService,
            IDelayFaultAttributionService delayFaultAttributionService,
            IOrderLoadSyncService orderLoadSyncService,
            INotificationService notifications)
        {
            _stops = stops;
            _loads = loads;
            _calculator = calculator;
            _activityLog = activityLog;
            _carrierPerformanceService = carrierPerformanceService;
            _etaPredictionService = etaPredictionService;
            _alertService = alertService;
            _delayFaultAttributionService = delayFaultAttributionService;
            _orderLoadSyncService = orderLoadSyncService;
            _notifications = notifications;
        }

        // PICKUP / DELIVERY WORKFLOW

        public async Task MarkEnRouteAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.Status != StopStatus.Pending)
                throw new BusinessRuleException("Stop can be marked EnRoute only from Pending.");

            var oldStatus = stop.Status;
            stop.Status = StopStatus.EnRoute;
            var etaResult = await _etaPredictionService.EvaluateAsync(stop);


            if (etaResult?.IsAtRisk == true)
            {
                // sink fields into LoadStop (single source of truth)
                stop.IsAtRiskOfDelay = true;
                stop.MinutesLatePrediction = etaResult.MinutesLate;
                stop.DelayRisk = etaResult.RiskLevel;

                await _alertService.HandleEtaDelayAsync(stop);
                await _delayFaultAttributionService.EvaluateAsync(stop);

            }


            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked EnRoute");
        }

        public async Task MarkArrivedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.Status != StopStatus.EnRoute)
                throw new BusinessRuleException("Stop must be EnRoute before Arrived.");

            var oldStatus = stop.Status;
            stop.ActualArrival ??= DateTime.UtcNow;
            stop.Status = StopStatus.Arrived;
            CalculateOnTimeMetrics(stop);



            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Arrived");
        }

        public async Task MarkLoadedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.StopType != StopType.Pickup)
                throw new BusinessRuleException("Only pickup stops can be marked as Loaded.");

            if (stop.Status != StopStatus.Arrived)
                throw new BusinessRuleException("Pickup stop must be Arrived before Loaded.");

            var oldStatus = stop.Status;

            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Loaded;
            await _carrierPerformanceService.RecordStopPerformanceAsync(stop);


            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Loaded");
        }

        public async Task MarkUnloadedAsync(Guid stopId, Guid userId)
        {
            var stop = await GetStopOrThrow(stopId);
            EnsureNotCompleted(stop);

            if (stop.StopType != StopType.Delivery)
                throw new BusinessRuleException("Only delivery stops can be marked as Unloaded.");


            if (stop.Status != StopStatus.Arrived)
                throw new BusinessRuleException("Delivery stop must be Arrived before Unloaded.");
            if (stop.Load.Status == LoadStatus.Delivered)
                throw new BusinessRuleException("Load already delivered.");

            var oldStatus = stop.Status;

            stop.ActualDeparture = DateTime.UtcNow;
            stop.Status = StopStatus.Completed;
            await _carrierPerformanceService.RecordStopPerformanceAsync(stop);


            await UpdateLoadStatusAndLogAsync(stop, oldStatus, userId, "marked Unloaded");
        }

        // CORE HELPER ( SINGLE SOURCE)
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
            await _orderLoadSyncService.SyncFromLoadAsync(load);

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

            await _notifications.NotifyLoadStopEventAsync(
                load.Id,
                userId,
                $"Stop #{stop.Sequence} ({stop.StopType}) {action}: {oldStopStatus} -> {stop.Status}");

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




        // PRIVATE GUARDS
        private async Task<LoadStop> GetStopOrThrow(Guid stopId)
            => await _stops.GetByIdWithLoadAsync(stopId)
                ?? throw new NotFoundException("Load stop not found.");

        private static void EnsureNotCompleted(LoadStop stop)
        {
            if (stop.Load.Status == LoadStatus.Completed)
                throw new BusinessRuleException("Completed load execution is locked.");

            if (stop.Status == StopStatus.Completed)
                throw new BusinessRuleException("Completed stop cannot be changed.");
        }

        private void CalculateOnTimeMetrics(LoadStop stop)
        {
            if (!stop.PlannedArrivalTo.HasValue || !stop.ActualArrival.HasValue)
                return;

            var flex = TimeSpan.FromMinutes(stop.FlexMinutes ?? 0);
            var allowedArrival = stop.PlannedArrivalTo.Value.Add(flex);

            var isOnTime = stop.ActualArrival.Value <= allowedArrival;

            stop.IsLateArrival = !isOnTime;

            var load = stop.Load;

            if (stop.StopType == StopType.Pickup)
            {
                load.OnTimePickup = !load.Stops
                    .Where(s => s.StopType == StopType.Pickup && s.ActualArrival.HasValue)
                    .Any(s =>
                        s.ActualArrival >
                        s.PlannedArrivalTo!.Value.Add(
                            TimeSpan.FromMinutes(s.FlexMinutes ?? 0)));
            }

            if (stop.StopType == StopType.Delivery)
            {
                load.OnTimeDelivery = !load.Stops
                    .Where(s => s.StopType == StopType.Delivery && s.ActualArrival.HasValue)
                    .Any(s =>
                        s.ActualArrival >
                        s.PlannedArrivalTo!.Value.Add(
                            TimeSpan.FromMinutes(s.FlexMinutes ?? 0)));
            }
        }
    }

}
