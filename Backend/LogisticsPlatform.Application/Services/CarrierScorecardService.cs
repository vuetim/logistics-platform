using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;


namespace LogisticsPlatform.Application.Services
{
    public class CarrierScoreCardService
    {
        private readonly ICarrierPerformanceRepository _repo;

        public CarrierScoreCardService(ICarrierPerformanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<CarrierScorecCardDto> GetScorecardAsync(Guid carrierId)
        {
            var records = await _repo.GetByCarrierAsync(carrierId);

            if (!records.Any())
                return new CarrierScorecCardDto { CarrierId = carrierId };

            var onTimeStops = records.Count(r => r.IsOnTime);

            return new CarrierScorecCardDto
            {
                CarrierId = carrierId,
                TotalStops = records.Count,
                OnTimeStops = onTimeStops,
                OnTimePercentage =
                    Math.Round(onTimeStops * 100.0 / records.Count, 2),
                AvgMinutesLate = (int)(
                    records
                        .Where(r => r.MinutesLate.HasValue)
                        .Average(r => r.MinutesLate) ?? 0)
            };
        }
    }

}
