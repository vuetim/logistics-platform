using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierQueryRepository : ICarrierQueryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Carrier> _set;

        public CarrierQueryRepository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<Carrier>();
        }

        public async Task<PagedResult<Carrier>> GetPagedAsync(CarrierQueryParameters parameters)
        {
            var query = _set.AsQueryable();

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(s) ||
                    c.McNumber.ToLower().Contains(s) ||
                    c.DotNumber.ToLower().Contains(s) ||
                    c.Email.ToLower().Contains(s) ||
                    c.Phone.ToLower().Contains(s)
                );
            }

            // 🎯 FILTER BY STATUS
            if (!string.IsNullOrWhiteSpace(parameters.Status))
            {
                query = query.Where(c => c.Status == parameters.Status);
            }

            // ⭐ FILTER BY RATING
            if (parameters.MinRating.HasValue)
            {
                query = query.Where(c => c.Rating >= parameters.MinRating.Value);
            }

            if (parameters.MaxRating.HasValue)
            {
                query = query.Where(c => c.Rating <= parameters.MaxRating.Value);
            }

            // 🔽 SORTING
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortDir.ToLower() == "desc"
                    ? query.OrderByDescendingDynamic(parameters.SortBy)
                    : query.OrderByDynamic(parameters.SortBy);
            }

            // 📊 TOTAL COUNT
            var totalCount = await query.CountAsync();

            // 📄 PAGINATION
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Carrier>(
                items,
                totalCount,
                parameters.Page,
                parameters.PageSize
            );
        }
    }
}
