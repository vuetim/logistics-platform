using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
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

        public async Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters)
        {
            var query = _context.Loads
                .Include(x => x.Customer)
                .Include(x => x.Carrier)
                .Include(x => x.Stops)     
                .AsQueryable();

            // 🔎 Search
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var s = parameters.Search.ToLower();
                query = query.Where(l =>
                    l.LoadNumber.ToLower().Contains(s) ||
                    l.Customer.Name.ToLower().Contains(s) ||
                    (l.Carrier != null && l.Carrier.Name.ToLower().Contains(s))
                );
            }

            //  FILTERS 
            if (parameters.Status.HasValue)
                query = query.Where(l => l.Status == parameters.Status);

            if (parameters.CustomerId.HasValue)
                query = query.Where(l => l.CustomerId == parameters.CustomerId);

            if (parameters.CarrierId.HasValue)
                query = query.Where(l => l.CarrierId == parameters.CarrierId);

            if (parameters.Mode.HasValue)
                query = query.Where(l => l.Mode == parameters.Mode);

            if (parameters.PickupFrom.HasValue)
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Pickup &&
                        s.PlannedDate >= parameters.PickupFrom));

            if (parameters.PickupTo.HasValue)
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Pickup &&
                        s.PlannedDate <= parameters.PickupTo));

            if (parameters.DeliveryFrom.HasValue)
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Delivery &&
                        s.PlannedDate >= parameters.DeliveryFrom));

            if (parameters.DeliveryTo.HasValue)
                query = query.Where(l =>
                    l.Stops.Any(s =>
                        s.StopType == StopType.Delivery &&
                        s.PlannedDate <= parameters.DeliveryTo));

            // 🔽 Sorting
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

                    PickupDate = l.Stops
                        .Where(s => s.StopType == StopType.Pickup)
                        .OrderBy(s => s.Sequence)
                        .Select(s => s.PlannedDate)
                        .FirstOrDefault(),

                    DeliveryDate = l.Stops
                        .Where(s => s.StopType == StopType.Delivery)
                        .OrderByDescending(s => s.Sequence)
                        .Select(s => s.PlannedDate)
                        .FirstOrDefault(),

                    CustomerRate = l.CustomerRate,
                    CarrierRate = l.CarrierRate,
                    Margin = l.CustomerRate - l.CarrierRate
                })
                .ToListAsync();

            return new PagedResult<LoadListItemDto>(
                items,
                total,
                parameters.Page,
                parameters.PageSize
            );
        }

        public async Task<LoadDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await _context.Loads
                .Include(l => l.Customer)
                .Include(l => l.Carrier)
                .Where(l => l.Id == id)
                .Select(l => new LoadDetailsDto
                {
                    Id = l.Id,
                    LoadNumber = l.LoadNumber,
                    Status = l.Status,

                    ModeType = l.Mode,

                    CustomerId = l.CustomerId,
                    CustomerName = l.Customer.Name,

                    CarrierId = l.CarrierId,
                    CarrierName = l.Carrier != null ? l.Carrier.Name : null,

                    Origin = l.Origin,
                    Destination = l.Destination,

                    PickupDate = l.Stops
.Where(s => s.StopType == StopType.Pickup)
.OrderBy(s => s.Sequence)
.Select(s => s.PlannedDate)
.FirstOrDefault(),

                    DeliveryDate = l.Stops
.Where(s => s.StopType == StopType.Delivery)
.OrderByDescending(s => s.Sequence)
.Select(s => s.PlannedDate)
.FirstOrDefault(),


                    CustomerRate = l.CustomerRate,
                    CarrierRate = l.CarrierRate,
                    Accessorials = l.Accessorials,

                    EquipmentTypes = l.Equipment
    .Select(e => e.EquipmentType)
    .Distinct()
    .ToList()
                })

                .FirstOrDefaultAsync();
        }
    }
}
