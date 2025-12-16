using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadStop;

public class LoadStopListDto
{
    public Guid Id { get; set; }
    public StopType StopType { get; set; }
    public int Sequence { get; set; }

    public string LocationName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public DateTime? AppointmentFrom { get; set; }
    public DateTime? AppointmentTo { get; set; }
    public bool HasTime { get; set; }

    public DateTime? PredictedArrivalAt { get; set; }
    public bool IsAtRiskOfDelay { get; set; }
    public string? DelayRiskReason { get; set; }
    public DelayRiskLevel DelayRisk { get; set; }
    public int? MinutesLatePrediction { get; set; }


    public static LoadStopListDto FromEntity(
        Domain.Entities.LoadStop stop)
    {
        return new LoadStopListDto
        {
            Id = stop.Id,
            Sequence = stop.Sequence,
            StopType = stop.StopType,

            LocationName = stop.LocationName,
            City = stop.City,
            State = stop.State,
            PostalCode = stop.PostalCode,

            AppointmentFrom = stop.PlannedArrivalFrom,
            AppointmentTo = stop.PlannedArrivalTo,
            HasTime = stop.AppointmentType == AppointmentType.Appointment,

            PredictedArrivalAt = stop.PredictedArrivalAt,
            IsAtRiskOfDelay = stop.IsAtRiskOfDelay,
            DelayRiskReason = stop.DelayRiskReason,
            DelayRisk = stop.DelayRisk,
            MinutesLatePrediction = stop.MinutesLatePrediction,

        };
    }
}
