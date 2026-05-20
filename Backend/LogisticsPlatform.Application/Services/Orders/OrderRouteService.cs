using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

public class OrderRouteService : IOrderRouteService
{
    private readonly IOrderRepository _orders;
    private readonly IOrderRouteRepository _routes;

    public OrderRouteService(
        IOrderRepository orders,
        IOrderRouteRepository routes)
    {
        _orders = orders;
        _routes = routes;
    }

    public async Task<Guid> CreateAsync(Guid orderId, CreateOrderRouteDto dto)
    {
        var order = await _orders.GetByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found");

        var route = new OrderRoute
        {
            OrderId = orderId,
            Sequence = dto.Sequence,
            StopType = dto.StopType,

            LocationName = dto.LocationName,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            StopReference = dto.StopReference,
            AppointmentNumber = dto.AppointmentNumber,
            PONumbers = dto.PONumbers,
            PlannedArrivalFrom = dto.PlannedArrivalFrom,
            PlannedArrivalTo = dto.PlannedArrivalTo,
            AppointmentType = dto.AppointmentType,
            FlexMinutes = dto.FlexMinutes,
            TimeZone = NormalizeTimeZone(dto.TimeZone),
            AppointmentStatus = dto.AppointmentStatus,
            AppointmentConfirmed = dto.AppointmentConfirmed,
            AppointmentConfirmationNumber = dto.AppointmentConfirmationNumber,

            HasTime = dto.HasTime,
            CopyToLoad = dto.CopyToLoad,
            IsActive = true,
            Notes = dto.Notes
        };

        await _routes.AddAsync(route);
        await _routes.SaveChangesAsync();

        return route.Id;
    }

    public async Task<List<OrderRouteDto>> GetByOrderIdAsync(Guid orderId)
    {
        var routes = await _routes.GetByOrderIdAsync(orderId);

        return routes.Select(r => new OrderRouteDto
        {
            Id = r.Id,
            Sequence = r.Sequence,
            StopType = r.StopType,
            LocationName = r.LocationName,
            AddressLine1 = r.AddressLine1,
            AddressLine2 = r.AddressLine2,
            City = r.City,
            State = r.State,
            PostalCode = r.PostalCode,
            Country = r.Country,
            Latitude = r.Latitude,
            Longitude = r.Longitude,
            PlannedArrivalFrom = r.PlannedArrivalFrom,
            PlannedArrivalTo = r.PlannedArrivalTo,
            AppointmentType = r.AppointmentType,
            FlexMinutes = r.FlexMinutes,
            TimeZone = r.TimeZone,
            AppointmentStatus = r.AppointmentStatus,
            AppointmentConfirmed = r.AppointmentConfirmed,
            AppointmentConfirmationNumber = r.AppointmentConfirmationNumber,
            HasTime = r.HasTime,
            CopyToLoad = r.CopyToLoad,
            StopReference = r.StopReference,
            AppointmentNumber = r.AppointmentNumber,
            PONumbers = r.PONumbers,
            Notes = r.Notes,
            IsActive = r.IsActive
        }).ToList();
    }

    public async Task UpdateAsync(Guid routeId, UpdateOrderRouteDto dto)
    {
        var route = await _routes.GetByIdAsync(routeId)
            ?? throw new NotFoundException("Route not found");

        route.Sequence = dto.Sequence ?? route.Sequence;
        route.StopType = dto.StopType ?? route.StopType;

        route.LocationName = dto.LocationName ?? route.LocationName;
        route.AddressLine1 = dto.AddressLine1 ?? route.AddressLine1;
        route.AddressLine2 = dto.AddressLine2 ?? route.AddressLine2;
        route.City = dto.City ?? route.City;
        route.State = dto.State ?? route.State;
        route.PostalCode = dto.PostalCode ?? route.PostalCode;
        route.Country = dto.Country ?? route.Country;
        route.Latitude = dto.Latitude ?? route.Latitude;
        route.Longitude = dto.Longitude ?? route.Longitude;

        route.PlannedArrivalFrom = dto.PlannedArrivalFrom ?? route.PlannedArrivalFrom;
        route.PlannedArrivalTo = dto.PlannedArrivalTo ?? route.PlannedArrivalTo;
        route.AppointmentType = dto.AppointmentType ?? route.AppointmentType;
        route.FlexMinutes = dto.FlexMinutes ?? route.FlexMinutes;
        route.TimeZone = NormalizeTimeZone(dto.TimeZone ?? route.TimeZone);
        route.AppointmentStatus = dto.AppointmentStatus ?? route.AppointmentStatus;
        route.AppointmentConfirmed = dto.AppointmentConfirmed ?? route.AppointmentConfirmed;
        route.AppointmentConfirmationNumber = dto.AppointmentConfirmationNumber ?? route.AppointmentConfirmationNumber;
        route.StopReference = dto.StopReference ?? route.StopReference;
        route.AppointmentNumber = dto.AppointmentNumber ?? route.AppointmentNumber;
        route.PONumbers = dto.PONumbers ?? route.PONumbers;
  

    route.HasTime = dto.HasTime ?? route.HasTime;
        route.CopyToLoad = dto.CopyToLoad ?? route.CopyToLoad;
        route.Notes = dto.Notes ?? route.Notes;

        await _routes.UpdateAsync(route);
        await _routes.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid routeId)
    {
        var route = await _routes.GetByIdAsync(routeId)
            ?? throw new NotFoundException("Route not found");

        await _routes.DeleteAsync(route);
        await _routes.SaveChangesAsync();
    }

    private static string NormalizeTimeZone(string? timeZone)
        => string.IsNullOrWhiteSpace(timeZone) ? "UTC" : timeZone.Trim();
}
