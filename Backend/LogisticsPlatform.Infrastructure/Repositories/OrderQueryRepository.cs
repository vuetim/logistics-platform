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

                    PreferredCarrierId = o.PreferredCarrierId,
                    PreferredCarrierName = o.PreferredCarrier != null
                        ? o.PreferredCarrier.Name
                        : null,

                    OrderType = o.OrderType,
                    Direction = o.Direction,
                    Status = o.Status,
                    Phase = o.Phase,

                    StartDate = new OrderDateDto
                    {
                        Date = o.StartDate,
                        Timezone = "UTC",
                        HasTime = true
                    },
                    EndDate = new OrderDateDto
                    {
                        Date = o.EndDate,
                        Timezone = "UTC",
                        HasTime = true
                    },
                    StartDateType = new LookupValueDto { Key = "33091", Value = "On a specific date" },
                    EndDateType = new LookupValueDto { Key = "33091", Value = "On a specific date" },
                    PlannedPickup = o.PlannedPickupDate.HasValue
                        ? new OrderDateDto
                        {
                            Date = o.PlannedPickupDate.Value,
                            Timezone = "UTC",
                            HasTime = true
                        }
                        : null,
                    PlannedDelivery = o.PlannedDeliveryDate.HasValue
                        ? new OrderDateDto
                        {
                            Date = o.PlannedDeliveryDate.Value,
                            Timezone = "UTC",
                            HasTime = true
                        }
                        : null,

                    Origin = o.OrderRoutes
                        .Where(r => r.IsActive && r.StopType == LogisticsPlatform.Domain.Enums.StopType.Pickup)
                        .OrderBy(r => r.Sequence)
                        .Select(r => r.LocationName)
                        .FirstOrDefault() ?? string.Empty,
                    Destination = o.OrderRoutes
                        .Where(r => r.IsActive && r.StopType == LogisticsPlatform.Domain.Enums.StopType.Delivery)
                        .OrderByDescending(r => r.Sequence)
                        .Select(r => r.LocationName)
                        .FirstOrDefault() ?? string.Empty,

                    DispatchNotes = o.DispatchNotes,
                    DeliveryNotes = o.DeliveryNotes,
                    CustomerRate = o.CustomerRate,
                    BaseFreight = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer && li.Type == LogisticsPlatform.Domain.Enums.ChargeType.Linehaul)
                            .Sum(li => li.Amount)
                        : 0m,
                    Accessorials = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer && li.Type != LogisticsPlatform.Domain.Enums.ChargeType.Linehaul)
                            .Sum(li => li.Amount)
                        : 0m,
                    QuotedTotal = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer)
                            .Sum(li => li.Amount)
                          + (o.Cost.LineItems
                                .Where(li => li.IsCustomer)
                                .Sum(li => li.Amount) * o.Cost.TaxRate / 100m)
                        : 0m,
                    PrimaryPONumber = o.PrimaryPONumber,
                    PrimaryBolNumber = o.PrimaryBolNumber,
                    PrimaryProNumber = o.PrimaryProNumber,
                    Commodity = o.Commodity,
                    TotalWeight = o.TotalWeight,
                    TotalPallets = o.TotalPallets,
                    TotalVolume = o.TotalVolume,
                    HasActiveLoad = o.Loads.Any(l => l.Load != null && !l.Load.IsArchived),
                    ActiveLoadId = o.Loads
                        .Where(l => l.Load != null && !l.Load.IsArchived)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => (Guid?)l.LoadId)
                        .FirstOrDefault(),
                    ActiveLoadNumber = o.Loads
                        .Where(l => l.Load != null && !l.Load.IsArchived)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.Load.LoadNumber)
                        .FirstOrDefault(),

                    Items = o.Items
                    .OrderBy(i => i.CreatedAt)
                    .Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        CustomerReference = i.CustomerReference,
                        IdentificationNumber = i.IdentificationNumber,
                        Quantity = i.Quantity,
                        ActualQuantity = i.HandlingQuantity ?? i.Quantity,
                        Status = "Active",
                        LineItemNumber = 0,
                        QuantityUnit = i.QuantityUnit,
                        IsHazmat = i.IsHazmat,
                        FreightClass = i.FreightClass,
                        HazardClass = i.HazardClass,
                        Notes = i.Notes
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

            if (parameters.Phase.HasValue)
                query = query.Where(o => o.Phase == parameters.Phase);

            if (parameters.Direction.HasValue)
                query = query.Where(o => o.Direction == parameters.Direction);

            if (parameters.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == parameters.CustomerId);

            if (parameters.PreferredCarrierId.HasValue)
                query = query.Where(o => o.PreferredCarrierId == parameters.PreferredCarrierId);

            if (parameters.FromDate.HasValue)
            {
                var from = parameters.FromDate.Value;
                query = query.Where(o =>
                    o.StartDate >= from ||
                    (o.PlannedPickupDate.HasValue && o.PlannedPickupDate.Value >= from));
            }

            if (parameters.ToDate.HasValue)
            {
                var to = parameters.ToDate.Value;
                query = query.Where(o =>
                    o.EndDate <= to ||
                    (o.PlannedDeliveryDate.HasValue && o.PlannedDeliveryDate.Value <= to));
            }

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
                    PreferredCarrierName = o.PreferredCarrier != null ? o.PreferredCarrier.Name : null,
                    Status = o.Status,
                    Phase = o.Phase,
                    Direction = o.Direction,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    PlannedPickupDate = o.PlannedPickupDate,
                    PlannedDeliveryDate = o.PlannedDeliveryDate,
                    Origin = o.OrderRoutes
                        .Where(r => r.IsActive && r.StopType == LogisticsPlatform.Domain.Enums.StopType.Pickup)
                        .OrderBy(r => r.Sequence)
                        .Select(r => r.LocationName)
                        .FirstOrDefault() ?? string.Empty,
                    Destination = o.OrderRoutes
                        .Where(r => r.IsActive && r.StopType == LogisticsPlatform.Domain.Enums.StopType.Delivery)
                        .OrderByDescending(r => r.Sequence)
                        .Select(r => r.LocationName)
                        .FirstOrDefault() ?? string.Empty,
                    BaseFreight = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer && li.Type == LogisticsPlatform.Domain.Enums.ChargeType.Linehaul)
                            .Sum(li => li.Amount)
                        : 0m,
                    Accessorials = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer && li.Type != LogisticsPlatform.Domain.Enums.ChargeType.Linehaul)
                            .Sum(li => li.Amount)
                        : 0m,
                    QuotedTotal = o.Cost != null
                        ? o.Cost.LineItems
                            .Where(li => li.IsCustomer)
                            .Sum(li => li.Amount)
                          + (o.Cost.LineItems
                                .Where(li => li.IsCustomer)
                                .Sum(li => li.Amount) * o.Cost.TaxRate / 100m)
                        : 0m,
                    Commodity = o.Commodity,
                    PrimaryPONumber = o.PrimaryPONumber,
                    PrimaryBolNumber = o.PrimaryBolNumber,
                    PrimaryProNumber = o.PrimaryProNumber,
                    TotalWeight = o.TotalWeight,
                    TotalPallets = o.TotalPallets,
                    TotalVolume = o.TotalVolume,
                    HasActiveLoad = o.Loads.Any(l => l.Load != null && !l.Load.IsArchived),
                    ActiveLoadId = o.Loads
                        .Where(l => l.Load != null && !l.Load.IsArchived)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => (Guid?)l.LoadId)
                        .FirstOrDefault(),
                    ActiveLoadNumber = o.Loads
                        .Where(l => l.Load != null && !l.Load.IsArchived)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.Load.LoadNumber)
                        .FirstOrDefault(),
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                })
                .ToListAsync();

            return new PagedResult<OrderListDto>(orders,
                total,
                parameters.Page,
                parameters.PageSize);
        }
    }
}
