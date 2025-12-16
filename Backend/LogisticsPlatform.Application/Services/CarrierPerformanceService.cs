using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierPerformanceService : ICarrierPerformanceService
    {
        private readonly ICarrierPerformanceRepository _repo;

        public CarrierPerformanceService(ICarrierPerformanceRepository repo)
        {
            _repo = repo;
        }

        public async Task RecordStopPerformanceAsync(LoadStop stop)
        {
            // Carrier i domosdoshëm
            if (!stop.Load.CarrierId.HasValue)
                return;

            // Pa planning → s’ka performance
            if (!stop.PlannedArrivalFrom.HasValue || !stop.PlannedArrivalTo.HasValue)
                return;

            // Pa actual arrival → s’ka performance
            if (!stop.ActualArrival.HasValue)
                return;

            var plannedFrom = stop.PlannedArrivalFrom.Value;
            var plannedTo = stop.PlannedArrivalTo.Value;
            var actual = stop.ActualArrival.Value;

            var isOnTime = actual >= plannedFrom && actual <= plannedTo;

            int? minutesLate = null;
            if (!isOnTime && actual > plannedTo)
            {
                minutesLate = (int)(actual - plannedTo).TotalMinutes;
            }

            var performance = new CarrierStopPerformance
            {
                CarrierId = stop.Load.CarrierId.Value,
                LoadId = stop.Load.Id,
                LoadStopId = stop.Id,
                StopType = stop.StopType,

                IsOnTime = isOnTime,
                IsLate = !isOnTime,
                MinutesLate = minutesLate,

                PlannedFrom = plannedFrom,
                PlannedTo = plannedTo,
                ActualArrival = actual
            };

            await _repo.AddAsync(performance);
            await _repo.SaveChangesAsync();
        }
    }
}
