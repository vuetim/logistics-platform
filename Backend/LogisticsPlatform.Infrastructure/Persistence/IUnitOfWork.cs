using LogisticsPlatform.Application.DTOs.Common;
using Microsoft.EntityFrameworkCore.Storage;


namespace LogisticsPlatform.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _tx;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginAsync()
        {
            _tx = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if (_tx != null)
                await _tx.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_tx != null)
                await _tx.RollbackAsync();
        }
        public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }

}
