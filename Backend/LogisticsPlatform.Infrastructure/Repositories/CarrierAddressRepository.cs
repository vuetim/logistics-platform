using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierAddressRepository : ICarrierAddressRepository
    {
        private readonly AppDbContext _context;

        public CarrierAddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CarrierAddress address)
        {
            await _context.CarrierAddresses.AddAsync(address);
        }

        public async Task UpdateAsync(CarrierAddress address)
        {
            _context.CarrierAddresses.Update(address);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(CarrierAddress address)
        {
            _context.CarrierAddresses.Remove(address);
            await Task.CompletedTask;
        }

        public async Task<CarrierAddress?> GetByIdAsync(Guid id)
        {
            return await _context.CarrierAddresses
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<CarrierAddress>> GetByCarrierAsync(Guid carrierId)
        {
            return await _context.CarrierAddresses
                .Where(a => a.CarrierId == carrierId)
                .ToListAsync();
        }
        public async Task<List<CarrierAddress>> GetPrimaryByCarrierAsync(Guid carrierId)
        {
            return await _context.CarrierAddresses
                .Where(x => x.CarrierId == carrierId && x.IsPrimary)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
