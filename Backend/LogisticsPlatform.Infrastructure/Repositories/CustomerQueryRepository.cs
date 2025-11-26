using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class CustomerQueryRepository : ICustomerQueryRepository
    {
        private readonly AppDbContext _context;

        public CustomerQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Customer>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Customers.AsQueryable();

            // 🔎 SEARCH
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(s) ||
                    c.Email.ToLower().Contains(s) ||
                    c.Phone.ToLower().Contains(s)
                );
            }

            // 🔽 SORTING
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                bool desc = parameters.SortDir?.ToLower() == "desc";

                query = desc
                    ? query.OrderByDescendingDynamic(parameters.SortBy)
                    : query.OrderByDynamic(parameters.SortBy);
            }

            // TOTAL
            var total = await query.CountAsync();

            // PAGING
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Customer>(items, total, parameters.Page, parameters.PageSize);
        }
    }
}
