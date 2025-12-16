using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Loads
{
    public interface IDelayResponsibilityRepository
    {
        Task AddAsync(DelayResponsibility entity);
        Task<List<DelayResponsibility>> GetByLoadAsync(Guid loadId);
        Task<DelayResponsibility?> GetLatestForStopAsync(Guid loadStopId);
        Task SaveChangesAsync();
    }

}
