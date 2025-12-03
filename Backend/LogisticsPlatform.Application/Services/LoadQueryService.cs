using LogisticsPlatform.Application.DTOs.Loads;
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

        public LoadQueryService(ILoadQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<LoadListItemDto>> GetPagedAsync(LoadQueryParameters parameters)
        {
            // vend për guardrails, defaults, security
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

            return new LoadDetailsDto
            {
                Execution = new LoadExecutionDetailsDto
                {
                    Id = load.Id,
                    LoadNumber = load.LoadNumber,
                    Status = load.Status,
                    Mode = load.Mode,

                    Origin = load.Origin,
                    Destination = load.Destination,

                    CarrierName = load.Carrier?.Name,

                    CustomerRate = load.CustomerRate,
                    CarrierRate = load.CarrierRate,

                    // ✅ STOPS = EXECUTION ONLY
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
    }
    };
        }
    }
}
