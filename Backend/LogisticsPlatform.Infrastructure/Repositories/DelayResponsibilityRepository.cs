using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;

namespace LogisticsPlatform.Infrastructure.Repositories
    {
        public class DelayResponsibilityRepository : IDelayResponsibilityRepository
        {
            private readonly AppDbContext _ctx;

            public DelayResponsibilityRepository(AppDbContext ctx)
            {
                _ctx = ctx;
            }

            public async Task AddAsync(DelayResponsibility entity)
                => await _ctx.DelayResponsibilities.AddAsync(entity);

            public async Task<List<DelayResponsibility>> GetByLoadAsync(Guid loadId)
                => await _ctx.DelayResponsibilities
                    .Where(x => x.LoadId == loadId)
                    .OrderByDescending(x => x.AssignedAt)
                    .ToListAsync();

            public async Task<DelayResponsibility?> GetLatestForStopAsync(Guid loadStopId)
                => await _ctx.DelayResponsibilities
                    .Where(x => x.LoadStopId == loadStopId)
                    .OrderByDescending(x => x.AssignedAt)
                    .FirstOrDefaultAsync();

            public async Task SaveChangesAsync()
                => await _ctx.SaveChangesAsync();
        }
    }


