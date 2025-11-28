using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class LoadNoteRepository : ILoadNoteRepository
    {
        private readonly AppDbContext _context;

        public LoadNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoadNote note)
        {
            _context.LoadNotes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LoadNote>> GetByLoadIdAsync(Guid loadId)
        {
            return await _context.LoadNotes
                   .Include(n => n.CreatedByUser)
                   .Where(n => n.LoadId == loadId)
                   .OrderByDescending(n => n.CreatedAt)
                   .ToListAsync();
        }
    }

}
