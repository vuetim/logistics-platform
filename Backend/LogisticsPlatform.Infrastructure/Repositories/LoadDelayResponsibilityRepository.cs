using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class LoadDelayResponsibilityRepository
       : ILoadDelayResponsibilityRepository
    {
        private readonly AppDbContext _ctx;

        public LoadDelayResponsibilityRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(LoadDelayResponsibility entity)
            => await _ctx.LoadDelayResponsibilities.AddAsync(entity);

        public async Task SaveChangesAsync()
            => await _ctx.SaveChangesAsync();
    }

}
