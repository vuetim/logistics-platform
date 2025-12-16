using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Carriers
{
    public interface ICarrierPerformanceRepository
    {
        Task AddAsync(CarrierStopPerformance performance);
        Task SaveChangesAsync();

        Task<List<CarrierStopPerformance>> GetByCarrierAsync(Guid carrierId);
    }

}
