using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class LoadStopDetailsDto
{
    // ======================
    // CORE
    // ======================
    public Guid Id { get; set; }
    public int Sequence { get; set; }
    public StopType StopType { get; set; }
    public StopStatus Status { get; set; }

    // ======================
    // LOCATION SNAPSHOT
    // ======================
    public string LocationName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    // ======================
    // PLANNED (order snapshot)
    // ======================
    public DateTime? PlannedArrivalFrom { get; set; }
    public DateTime? PlannedArrivalTo { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public int? FlexMinutes { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public AppointmentStatus AppointmentStatus { get; set; }
    public bool AppointmentConfirmed { get; set; }
    public string? AppointmentConfirmationNumber { get; set; }
    public string? AppointmentNumber { get; set; }
    public string? StopReference { get; set; }
    public string? PONumbers { get; set; }

    // ======================
    // REVISED (dispatcher)
    // ======================
    public DateTime? RevisedArrivalFrom { get; set; }
    public DateTime? RevisedArrivalTo { get; set; }

    // ======================
    // ACTUAL (execution truth)
    // ======================
    public DateTime? ActualArrival { get; set; }
    public DateTime? ActualDeparture { get; set; }

    // ======================
    // KPI
    // ======================
    public bool? IsOnTime { get; set; }
    public int? MinutesLate { get; set; }


    // ======================
    // META
    // ======================
    public string? Notes { get; set; }

    public static LoadStopDetailsDto FromEntity(LoadStop stop)
    {
        return new LoadStopDetailsDto
        {
            Id = stop.Id,
            Sequence = stop.Sequence,
            StopType = stop.StopType,
            Status = stop.Status,

            LocationName = stop.LocationName,
            AddressLine1 = stop.AddressLine1,
            AddressLine2 = stop.AddressLine2,
            City = stop.City,
            State = stop.State,
            PostalCode = stop.PostalCode,
            Country = stop.Country,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,

            PlannedArrivalFrom = stop.PlannedArrivalFrom,
            PlannedArrivalTo = stop.PlannedArrivalTo,
            AppointmentType = stop.AppointmentType,
            FlexMinutes = stop.FlexMinutes,
            TimeZone = stop.TimeZone,
            AppointmentStatus = stop.AppointmentStatus,
            AppointmentConfirmed = stop.AppointmentConfirmed,
            AppointmentConfirmationNumber = stop.AppointmentConfirmationNumber,
            AppointmentNumber = stop.AppointmentNumber,
            StopReference = stop.StopReference,
            PONumbers = stop.PONumbers,

            RevisedArrivalFrom = stop.RevisedArrivalFrom,
            RevisedArrivalTo = stop.RevisedArrivalTo,

            ActualArrival = stop.ActualArrival,
            ActualDeparture = stop.ActualDeparture,

            IsOnTime = stop.IsOnTime,
            MinutesLate = stop.MinutesLate,
            Notes = stop.Notes
        };
    }
}
