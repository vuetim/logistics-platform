using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads.OrderToLoad;

public class OrderToLoadSnapshotBuilder : IOrderToLoadSnapshotBuilder
{
    private readonly ILoadCostSnapshotBuilder _costSnapshotBuilder;
    private readonly ILoadNumberGenerator _numberGenerator;

    public OrderToLoadSnapshotBuilder(
        ILoadCostSnapshotBuilder costSnapshotBuilder,
        ILoadNumberGenerator numberGenerator)
    {
        _costSnapshotBuilder = costSnapshotBuilder;
        _numberGenerator = numberGenerator;
    }

    public OrderToLoadSnapshot Build(Order order, CreateLoadFromOrderDto dto, Guid userId, SelectedOrderRoutes routes)
    {
        var costSnapshot = _costSnapshotBuilder.Build(order);
        var firstRoute = routes.First;
        var lastRoute = routes.Last;

        var load = new Load
        {
            LoadNumber = _numberGenerator.Generate(),
            CustomerId = order.CustomerId,
            CarrierId = dto.CarrierId ?? order.PreferredCarrierId,
            Status = LoadStatus.Draft,
            Mode = ModeType.TL,
            CustomerRate = costSnapshot.CustomerRate,
            CarrierRate = dto.CarrierRate ?? 0,
            Accessorials = costSnapshot.Accessorials,
            BolNumber = order.PrimaryBolNumber,
            ProNumber = order.PrimaryProNumber,
            RateConfirmationNumber = dto.RateConfirmationNumber,
            TrackingNumber = order.PrimaryProNumber,
            Origin = ResolveLaneName(firstRoute),
            Destination = ResolveLaneName(lastRoute),
            IsArchived = false,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            Cost = costSnapshot.Cost
        };

        var stops = routes.Routes
            .Select(route => BuildStop(load, route, firstRoute, lastRoute, dto))
            .ToList();

        foreach (var item in order.Items.Where(i => i.CopyToLoad))
        {
            load.Items.Add(BuildItem(load, item));
        }

        foreach (var requirement in order.EquipmentRequirements.Where(e => e.CopyToLoad))
        {
            load.Equipment.Add(EquipmentRequirementMapper.ToLoadEquipment(requirement, load));
        }

        load.HasEquipment = load.Equipment.Any();
        load.IsTemperatureControlled = load.Equipment.Any(e => e.EquipmentType == EquipmentType.Reefer);

        var loadOrder = new LoadOrder
        {
            Load = load,
            OrderId = order.Id,
            PONumber = !string.IsNullOrWhiteSpace(order.PrimaryPONumber)
                ? order.PrimaryPONumber
                : order.OrderNumber
        };

        return new OrderToLoadSnapshot(load, stops, loadOrder);
    }

    private static string ResolveLaneName(OrderRoute route)
        => !string.IsNullOrWhiteSpace(route.LocationName)
            ? route.LocationName
            : $"{route.City}, {route.State}";

    private static LoadStop BuildStop(
        Load load,
        OrderRoute route,
        OrderRoute firstRoute,
        OrderRoute lastRoute,
        CreateLoadFromOrderDto dto)
    {
        var plannedArrivalFrom = route.PlannedArrivalFrom;
        var plannedArrivalTo = route.PlannedArrivalTo;

        if (route.Id == firstRoute.Id && dto.PlannedPickupDate.HasValue)
        {
            plannedArrivalFrom = dto.PlannedPickupDate.Value;
            plannedArrivalTo = dto.PlannedPickupDate.Value;
        }

        if (route.Id == lastRoute.Id && dto.PlannedDeliveryDate.HasValue)
        {
            plannedArrivalFrom = dto.PlannedDeliveryDate.Value;
            plannedArrivalTo = dto.PlannedDeliveryDate.Value;
        }

        return new LoadStop
        {
            Load = load,
            Sequence = route.Sequence,
            StopType = route.StopType,
            LocationName = route.LocationName,
            AddressLine1 = route.AddressLine1,
            AddressLine2 = route.AddressLine2,
            City = route.City,
            State = route.State,
            PostalCode = route.PostalCode,
            Country = route.Country,
            Latitude = route.Latitude,
            Longitude = route.Longitude,
            PlannedArrivalFrom = plannedArrivalFrom,
            PlannedArrivalTo = plannedArrivalTo,
            PlannedDepartureFrom = route.PlannedDepartureFrom,
            PlannedDepartureTo = route.PlannedDepartureTo,
            AppointmentType = route.AppointmentType,
            FlexMinutes = route.FlexMinutes,
            TimeZone = route.TimeZone,
            AppointmentStatus = route.AppointmentStatus,
            AppointmentConfirmed = route.AppointmentConfirmed,
            AppointmentConfirmationNumber = route.AppointmentConfirmationNumber,
            StopReference = route.StopReference ?? string.Empty,
            AppointmentNumber = route.AppointmentNumber,
            PONumbers = route.PONumbers,
            Status = StopStatus.Pending,
            Notes = route.Notes
        };
    }

    private static LoadItem BuildItem(Load load, OrderItem orderItem)
        => new()
        {
            Load = load,
            SourceOrderItemId = orderItem.Id,
            Name = orderItem.Name,
            CustomerReference = orderItem.CustomerReference,
            Quantity = orderItem.Quantity,
            QuantityUnit = orderItem.QuantityUnit,
            HandlingQuantity = orderItem.HandlingQuantity,
            HandlingUnit = orderItem.HandlingUnit,
            UnitNetWeight = orderItem.UnitNetWeight,
            UnitGrossWeight = orderItem.UnitGrossWeight,
            WeightUnit = orderItem.WeightUnit,
            Length = orderItem.Length,
            Width = orderItem.Width,
            Height = orderItem.Height,
            DimensionUnit = orderItem.DimensionUnit,
            Volume = orderItem.Volume,
            VolumeUnit = orderItem.VolumeUnit,
            MinTemperature = orderItem.MinTemperature,
            MaxTemperature = orderItem.MaxTemperature,
            TemperatureUnit = orderItem.TemperatureUnit,
            IsHazmat = orderItem.IsHazmat,
            HazardClass = orderItem.HazardClass,
            IdentificationNumber = orderItem.IdentificationNumber,
            FreightClass = orderItem.FreightClass,
            DeclaredValue = orderItem.DeclaredValue,
            Currency = orderItem.Currency,
            Stackable = orderItem.Stackable,
            Notes = orderItem.Notes
        };
}
