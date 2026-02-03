using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
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

        public async Task<PagedResult<CustomerListItemDto>> GetPagedAsync(CustomersQueryParameters p)
        {
            var query = _context.Customers.AsNoTracking();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(p.Search))
            {
                var s = p.Search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(s) ||
                    (c.Email != null && c.Email.ToLower().Contains(s))
                );
            }

            // STATUS FILTER (KRYESORE)
            if (p.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == p.IsActive.Value);
            }

            // SORT
            if (!string.IsNullOrWhiteSpace(p.SortBy))
            {
                query = p.SortDir == "desc"
                    ? query.OrderByDescendingDynamic(p.SortBy)
                    : query.OrderByDynamic(p.SortBy);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .Select(c => new CustomerListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return new PagedResult<CustomerListItemDto>(
                items,
                total,
                p.Page,
                p.PageSize
            );
        }

    }
}
