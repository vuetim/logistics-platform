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

        public async Task<PagedResult<CustomerListItemDto>> GetPagedAsync(
            QueryParameters parameters
        )
        {
            var query = _context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(s) ||
                    c.Email!.ToLower().Contains(s) ||
                    c.Phone!.ToLower().Contains(s)
                );
            }

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortDir == "desc"
                    ? query.OrderByDescendingDynamic(parameters.SortBy)
                    : query.OrderByDynamic(parameters.SortBy);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(c => new CustomerListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    IsActive = true // ose nga column
                })
                .ToListAsync();

            return new PagedResult<CustomerListItemDto>(
                items,
                total,
                parameters.Page,
                parameters.PageSize
            );
        }
    }
}
