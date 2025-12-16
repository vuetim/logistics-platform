using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierPerformanceRepository : ICarrierPerformanceRepository
    {
        private readonly AppDbContext _ctx;

        public CarrierPerformanceRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(CarrierStopPerformance performance)
            => await _ctx.CarrierStopPerformances.AddAsync(performance);

        public async Task SaveChangesAsync()
            => await _ctx.SaveChangesAsync();

        public async Task<List<CarrierStopPerformance>> GetByCarrierAsync(Guid carrierId)
            => await _ctx.CarrierStopPerformances
                .Where(x => x.CarrierId == carrierId)
                .ToListAsync();
    }

}
