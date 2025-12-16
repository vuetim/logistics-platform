using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CarrierDocumentQueryRepository : ICarrierDocumentQueryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<CarrierDocument> _set;

        public CarrierDocumentQueryRepository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<CarrierDocument>();
        }

        public async Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters)
        {
            var query = _set.AsQueryable();

            // 🔎 UNIVERSAL SEARCH
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(d =>
                    d.FileName.ToLower().Contains(s) ||
                    d.DocumentType.ToLower().Contains(s) ||
                    d.FileUrl.ToLower().Contains(s)
                );
            }

            // 🎯 FILTER BY CARRIER ID
            if (parameters.CarrierId.HasValue)
                query = query.Where(d => d.CarrierId == parameters.CarrierId.Value);

            // 🎯 FILTER BY DOCUMENT TYPE
            if (!string.IsNullOrWhiteSpace(parameters.DocumentType))
                query = query.Where(d => d.DocumentType == parameters.DocumentType);

            // 🎯 FILTER EXPIRES BEFORE
            if (parameters.ExpiringBefore.HasValue)
                query = query.Where(d => d.ExpiresAt <= parameters.ExpiringBefore);

            // 🔽 SORTING
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortDir.ToLower() == "desc"
                    ? query.OrderByDescendingDynamic(parameters.SortBy)
                    : query.OrderByDynamic(parameters.SortBy);
            }

            // 📊 TOTAL COUNT
            var total = await query.CountAsync();

            // 📄 PAGINATION
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<CarrierDocument>(items, total, parameters.Page, parameters.PageSize);
        }
    }
}
