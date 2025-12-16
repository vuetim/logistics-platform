using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierNoteRepository : ICarrierNoteRepository
    {
        private readonly AppDbContext _context;

        public CarrierNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CarrierNote note)
        {
            await _context.CarrierNotes.AddAsync(note);
        }

        public async Task UpdateAsync(CarrierNote note)
        {
            _context.CarrierNotes.Update(note);
        }

        public async Task DeleteAsync(CarrierNote note)
        {
            _context.CarrierNotes.Remove(note);
        }

        public async Task<CarrierNote?> GetByIdAsync(Guid id)
        {
            return await _context.CarrierNotes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<CarrierNote>> GetByCarrierIdAsync(Guid carrierId)
        {
            return await _context.CarrierNotes
                .Where(n => n.CarrierId == carrierId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
