using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class LoadDocumentRepository : ILoadDocumentRepository
    {
        private readonly AppDbContext _context;

        public LoadDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoadDocument document)
        {
            await _context.LoadDocuments.AddAsync(document);
        }

        public async Task<IEnumerable<LoadDocument>> GetByLoadAsync(Guid loadId)
        {
            return await _context.LoadDocuments
                .Where(d => d.LoadId == loadId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<LoadDocument?> GetByIdAsync(Guid id)
        {
            return await _context.LoadDocuments.FindAsync(id);
        }

        public async Task DeleteAsync(LoadDocument document)
        {
            _context.LoadDocuments.Remove(document);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
