using LogisticsPlatform.Application.Interfaces.Repositories.Security;
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
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly AppDbContext _ctx;

        public PasswordResetTokenRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(PasswordResetToken token)
            => await _ctx.PasswordResetTokens.AddAsync(token);

        public Task<PasswordResetToken?> GetValidAsync(string token)
            => _ctx.PasswordResetTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.IsUsed &&
                    x.ExpiresAt > DateTime.UtcNow);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();
    }

}
