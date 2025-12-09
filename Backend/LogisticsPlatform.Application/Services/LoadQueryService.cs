using LogisticsPlatform.Application.DTOs.Costs;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Loads.LoadEquipment;
using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class LoadQueryService : ILoadQueryService
    {
        private readonly ILoadQueryRepository _repo;
        private readonly ILoadCostRepository _loadCostRepo;

        public LoadQueryService(
            ILoadQueryRepository repo,
            ILoadCostRepository loadCostRepo)
        {
            _repo = repo;
            _loadCostRepo = loadCostRepo;
        }

        public async Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters)
        {
            if (parameters.PageSize > 100)
                parameters.PageSize = 100;

            return await _repo.GetPagedAsync(parameters);
        }

        public async Task<LoadDetailsDto?> GetDetailsAsync(Guid loadId)
        {
            var load = await _repo.GetByIdAsync(loadId);
            if (load == null)
                return null;

            var orderLink = load.Orders.FirstOrDefault();

            // 🔥 LoadCost (nga db)
            var loadCost = await _loadCostRepo.GetByLoadIdAsync(loadId);

            var totalBillable = loadCost?
                .LineItems.Where(x => x.IsCustomer)
                .Sum(x => x.Amount) ?? 0;

            var totalPayable = loadCost?
                .LineItems.Where(x => x.IsCarrier)
                .Sum(x => x.Amount) ?? 0;

            return new LoadDetailsDto
            {
                // ==================
                // EXECUTION SECTION
                // ==================
                Execution = new LoadExecutionDetailsDto
                {
                    Id = load.Id,
                    LoadNumber = load.LoadNumber,
                    Status = load.Status,
                    Mode = load.Mode,

                    Origin = load.Origin,
                    Destination = load.Destination,
                    CustomerName = load.Customer.Name,
                    CarrierName = load.Carrier?.Name,

                    CustomerRate = load.CustomerRate,
                    CarrierRate = load.CarrierRate,

                    Stops = load.Stops
                        .OrderBy(s => s.Sequence)
                        .Select(s => new LoadStopDetailsDto
                        {
                            Id = s.Id,
                            Sequence = s.Sequence,
                            StopType = s.StopType,
                            Status = s.Status,
                            LocationName = s.LocationName,
                            AddressLine1 = s.AddressLine1,
                            AddressLine2 = s.AddressLine2,
                            City = s.City,
                            State = s.State,
                            PostalCode = s.PostalCode,
                            Country = s.Country,
                            PlannedArrivalFrom = s.PlannedArrivalFrom,
                            PlannedArrivalTo = s.PlannedArrivalTo,
                            AppointmentType = s.AppointmentType,
                            FlexMinutes = s.FlexMinutes,
                            RevisedArrivalFrom = s.RevisedArrivalFrom,
                            RevisedArrivalTo = s.RevisedArrivalTo,
                            ActualArrival = s.ActualArrival,
                            ActualDeparture = s.ActualDeparture,
                            Notes = s.Notes
                        })
                        .ToList()
                },

                // ==================
                // ORDER SNAPSHOT
                // ==================
                OrderSnapshot = orderLink?.Order == null
                    ? null
                    : new LoadOrderSnapshotDto
                    {
                        OrderId = orderLink.Order.Id,
                        OrderNumber = orderLink.Order.OrderNumber,
                        OrderType = orderLink.Order.OrderType,
                        Direction = orderLink.Order.Direction,
                        PlannedPickupDate = orderLink.Order.PlannedPickupDate,
                        PlannedDeliveryDate = orderLink.Order.PlannedDeliveryDate,

                        Routes = orderLink.Order.OrderRoutes
                            .Where(r => r.IsActive)
                            .OrderBy(r => r.Sequence)
                            .Select(r => new OrderRouteDto
                            {
                                Id = r.Id,
                                Sequence = r.Sequence,
                                StopType = r.StopType,
                                LocationName = r.LocationName,
                                City = r.City,
                                State = r.State,
                                Country = r.Country,
                                PlannedArrivalFrom = r.PlannedArrivalFrom,
                                PlannedArrivalTo = r.PlannedArrivalTo,
                                HasTime = r.HasTime,
                                CopyToLoad = r.CopyToLoad,
                                Notes = r.Notes,
                                IsActive = r.IsActive
                            })
                            .ToList()
                    },

                // ==================
                // EQUIPMENT
                // ==================
                Equipment = load.Equipment.Select(e => new LoadEquipmentDto
                {
                    Id = e.Id,
                    EquipmentType = e.EquipmentType,
                    Length = e.Length,
                    Weight = e.Weight,
                    WeightUnit = e.WeightUnit,
                    MinTemp = e.MinTemp,
                    MaxTemp = e.MaxTemp,
                    TempUnit = e.TempUnit
                }).ToList(),
                HasEquipment = load.HasEquipment,

                // ==================
                // ITEMS
                // ==================
                Items = load.Items.Select(i => new LoadItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    QuantityUnit = i.QuantityUnit,
                    IsHazmat = i.IsHazmat,
                    FreightClass = i.FreightClass,
                    Notes = i.Notes
                }).ToList(),

                // ==================
                // SUMMARY
                // ==================
                Summary = CalculateSummary(load),

                // ==================
                // COST SUMMARY
                // ==================
                CostSummary = new LoadCostSummaryDto
                {
                    CustomerRate = load.CustomerRate ?? 0,
                    CarrierRate = load.CarrierRate ?? 0,

                    TotalBillable =
        (load.CustomerRate ?? 0) +
        (load.Cost?.LineItems
            .Where(x => x.IsCustomer)
            .Sum(x => x.Amount) ?? 0),

                    TotalPayable =
        (load.CarrierRate ?? 0) +
        (load.Cost?.LineItems
            .Where(x => x.IsCarrier)
            .Sum(x => x.Amount) ?? 0),

                    Margin =
        ((load.CustomerRate ?? 0) +
         (load.Cost?.LineItems.Where(x => x.IsCustomer).Sum(x => x.Amount) ?? 0))
         -
        ((load.CarrierRate ?? 0) +
         (load.Cost?.LineItems.Where(x => x.IsCarrier).Sum(x => x.Amount) ?? 0))
                }
            };
        }

        // =============
        // SUMMARY LOGIC
        // =============
        private LoadSummaryDto CalculateSummary(Load load)
        {
            decimal totalWeight = 0;
            decimal totalVolume = 0;
            decimal totalPallets = 0;

            foreach (var item in load.Items)
            {
                var weight = item.UnitGrossWeight ?? item.UnitNetWeight ?? 0;
                totalWeight += weight * item.Quantity;

                if (item.Length.HasValue && item.Width.HasValue && item.Height.HasValue)
                {
                    totalVolume += item.Length.Value * item.Width.Value * item.Height.Value * item.Quantity;
                }

                if (item.HandlingQuantity.HasValue)
                {
                    totalPallets += item.HandlingQuantity.Value;
                }
            }

            var pickupStops = load.Stops.Where(s => s.StopType == StopType.Pickup).ToList();
            var deliveryStops = load.Stops.Where(s => s.StopType == StopType.Delivery).ToList();

            return new LoadSummaryDto
            {
                TotalWeight = totalWeight,
                TotalVolume = totalVolume,
                TotalPallets = totalPallets,
                TotalItems = load.Items.Count,
                TotalStops = load.Stops.Count,
                PickupStops = pickupStops.Count,
                DeliveryStops = deliveryStops.Count,
                PickupLocations = pickupStops.Select(s => $"{s.City}, {s.State}").ToList(),
                DeliveryLocations = deliveryStops.Select(s => $"{s.City}, {s.State}").ToList()
            };
        }
    }
}
