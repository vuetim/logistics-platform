using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly AppDbContext _context;

        public CarrierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Carrier>> GetAllAsync()
        {
            return await _context.Carriers.ToListAsync();
        }

        public async Task<Carrier?> GetByIdAsync(Guid id)
        {
            return await _context.Carriers.FindAsync(id);
        }

        public async Task AddAsync(Carrier carrier)
        {
            await _context.Carriers.AddAsync(carrier);
        }

        public async Task UpdateAsync(Carrier carrier)
        {
            _context.Carriers.Update(carrier);
        }

        public async Task DeleteAsync(Carrier carrier)
        {
            _context.Carriers.Remove(carrier);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
