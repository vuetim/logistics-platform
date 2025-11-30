using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierContactRepository : ICarrierContactRepository
    {
        private readonly AppDbContext _context;

        public CarrierContactRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CarrierContact contact)
        {
            await _context.CarrierContacts.AddAsync(contact);
        }

        public async Task UpdateAsync(CarrierContact contact)
        {
            _context.CarrierContacts.Update(contact);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(CarrierContact contact)
        {
            _context.CarrierContacts.Remove(contact);
            await Task.CompletedTask;
        }

        public async Task<CarrierContact?> GetByIdAsync(Guid id)
        {
            return await _context.CarrierContacts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CarrierContact>> GetByCarrierAsync(Guid carrierId)
        {
            return await _context.CarrierContacts
                .Where(c => c.CarrierId == carrierId)
                .ToListAsync();
        }
        public async Task<List<CarrierContact>> GetPrimaryByCarrierAsync(Guid carrierId)
        {
            return await _context.CarrierContacts
                .Where(x => x.CarrierId == carrierId && x.IsPrimary)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
