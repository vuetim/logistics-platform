using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Extensions;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class LoadQueryRepository : ILoadQueryRepository
    {
        private readonly AppDbContext _context;

        public LoadQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        // ====================================================
        // LIST VIEW (TABLE / SEARCH / FILTERS)
        // ====================================================
        public async Task<PagedResult<LoadListItemDto>> GetPagedAsync(
            LoadQueryParameters parameters)
        {
            var query = _context.Loads
                .Include(l => l.Customer)
                .Include(l => l.Carrier)
                .Include(l => l.Stops)
                .AsQueryable();

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(l =>
                    l.LoadNumber.ToLower().Contains(s) ||
                    l.Customer.Name.ToLower().Contains(s) ||
                    (l.Carrier != null && l.Carrier.Name.ToLower().Contains(s))
                );
            }

            // 🎯 FILTERS
            if (parameters.Status.HasValue)
                query = query.Where(l => l.Status == parameters.Status);

            if (parameters.CustomerId.HasValue)
                query = query.Where(l => l.CustomerId == parameters.CustomerId);

            if (parameters.CarrierId.HasValue)
                query = query.Where(l => l.CarrierId == parameters.CarrierId);

            if (parameters.Mode.HasValue)
                query = query.Where(l => l.Mode == parameters.Mode);
            if (parameters.Mode.HasValue)
                query = query.Where(l => l.Mode == parameters.Mode);

            if (parameters.PickupFrom.HasValue)
            {
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Pickup &&
                        (s.PlannedArrivalFrom >= parameters.PickupFrom ||
 s.PlannedArrivalTo >= parameters.PickupFrom)));
            }

            if (parameters.PickupTo.HasValue)
            {
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Pickup &&
                        (s.PlannedArrivalFrom <= parameters.PickupTo ||
                         s.PlannedArrivalTo <= parameters.PickupTo)));
            }


            if (parameters.DeliveryFrom.HasValue)
            {
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Delivery &&
                        (s.PlannedArrivalFrom >= parameters.DeliveryFrom ||
                         s.PlannedArrivalTo >= parameters.DeliveryFrom)));
            }


            if (parameters.DeliveryTo.HasValue)
            {
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Delivery &&
                        (s.PlannedArrivalFrom <= parameters.DeliveryTo ||
                         s.PlannedArrivalTo <= parameters.DeliveryTo)));
            }


            // 🔽 SORTING
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortDir == "desc"
                    ? query.OrderByDescendingDynamic(parameters.SortBy)
                    : query.OrderByDynamic(parameters.SortBy);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(l => new LoadListItemDto
                {
                    Id = l.Id,
                    LoadNumber = l.LoadNumber,
                    CustomerName = l.Customer.Name,
                    CarrierName = l.Carrier != null ? l.Carrier.Name : null,
                    Status = l.Status,
                    ModeType = l.Mode,
                    HasEquipment = l.HasEquipment,

                    PickupDate = l.Stops
                        .Where(s => s.StopType == StopType.Pickup)
                        .OrderBy(s => s.Sequence)
.Select(s => s.PlannedArrivalFrom ?? s.PlannedArrivalTo)
                        .FirstOrDefault(),

                    DeliveryDate = l.Stops
                        .Where(s => s.StopType == StopType.Delivery)
                        .OrderByDescending(s => s.Sequence)
.Select(s => s.PlannedArrivalFrom ?? s.PlannedArrivalTo)
                        .FirstOrDefault(),

                    CustomerRate = l.CustomerRate,
                    CarrierRate = l.CarrierRate,
                    TotalBillable =
                        (l.CustomerRate ?? 0) +
                        (l.Cost != null
                            ? l.Cost.LineItems
                                .Where(li => li.IsCustomer)
                                .Sum(li => li.Amount)
                            : 0),
                    TotalPayable =
                        (l.CarrierRate ?? 0) +
                        (l.Cost != null
                            ? l.Cost.LineItems
                                .Where(li => li.IsCarrier)
                                .Sum(li => li.Amount)
                            : 0),
                    Margin =
                        ((l.CustomerRate ?? 0) +
                         (l.Cost != null
                            ? l.Cost.LineItems
                                .Where(li => li.IsCustomer)
                                .Sum(li => li.Amount)
                            : 0))
                        -
                        ((l.CarrierRate ?? 0) +
                         (l.Cost != null
                            ? l.Cost.LineItems
                                .Where(li => li.IsCarrier)
                                .Sum(li => li.Amount)
                            : 0))
                })
                .ToListAsync();

            return new PagedResult<LoadListItemDto>(
                items,
                total,
                parameters.Page,
                parameters.PageSize
            );
        }

        // ====================================================
        // DETAILS (ENTITY GRAPH – NO DTO PROJECTION)
        // ====================================================
        public async Task<Load?> GetByIdAsync(Guid id)
        {
            return await _context.Loads
                .Include(l => l.Customer)
                .Include(l => l.Carrier)
                .Include(l => l.Stops)
                .Include(l => l.Items)
                .Include(l => l.Equipment)
                .Include(l => l.Notes)
                .Include(l => l.Documents)
                .Include(l => l.Orders)
                    .ThenInclude(lo => lo.Order)
                        .ThenInclude(o => o.OrderRoutes)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        // ====================================================
        // WRITE HELPER (LINK ORDER ↔ LOAD)
        // ====================================================
        public async Task AddLoadOrderAsync(LoadOrder loadOrder)
        {
            await _context.LoadOrders.AddAsync(loadOrder);
        }
    }
}
