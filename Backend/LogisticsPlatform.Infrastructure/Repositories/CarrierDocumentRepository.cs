using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierDocumentRepository : ICarrierDocumentRepository
    {
        private readonly AppDbContext _context;

        public CarrierDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CarrierDocument doc)
        {
            await _context.CarrierDocuments.AddAsync(doc);
        }

        public async Task UpdateAsync(CarrierDocument doc)
        {
            _context.CarrierDocuments.Update(doc);
        }

        public async Task DeleteAsync(CarrierDocument doc)
        {
            _context.CarrierDocuments.Remove(doc);
        }

        public async Task<CarrierDocument?> GetByIdAsync(Guid id)
        {
            return await _context.CarrierDocuments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<CarrierDocument>> GetByCarrierAsync(Guid carrierId)
        {
            return await _context.CarrierDocuments
                .Where(d => d.CarrierId == carrierId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
