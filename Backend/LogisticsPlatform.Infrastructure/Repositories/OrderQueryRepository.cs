using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Persistence.Repositories.Queries
{
    public class OrderQueryRepository : IOrderQueryRepository
    {
        private readonly AppDbContext _context;

        public OrderQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,

                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,

                    PreferredCarrierName = o.PreferredCarrier != null
                        ? o.PreferredCarrier.Name
                        : null,

                    OrderType = o.OrderType,
                    Direction = o.Direction,
                    Status = o.Status,
                    Phase = o.Phase,

                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    PlannedPickupDate = o.PlannedPickupDate,
                    PlannedDeliveryDate = o.PlannedDeliveryDate,

                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        Name = i.Name,
                        Quantity = i.Quantity,
                        QuantityUnit = i.QuantityUnit,
                        IsHazmat = i.IsHazmat
                    }).ToList(),

                    CreatedAt = o.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<OrderListDto>> GetPagedAsync(OrderQueryParameters parameters)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(o =>
                    o.OrderNumber.Contains(parameters.Search) ||
                    o.Customer.Name.Contains(parameters.Search));
            }

            if (parameters.Status.HasValue)
                query = query.Where(o => o.Status == parameters.Status);

            // Count
            var total = await query.CountAsync();

            // Page
            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(o => new OrderListDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.Customer.Name,
                    Status = o.Status,
                    Phase = o.Phase,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<OrderListDto>(orders,
                total,
                parameters.Page,
                parameters.PageSize);
        }
    }
}
