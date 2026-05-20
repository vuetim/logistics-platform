using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class LoadStop : BaseEntity
    {
        public Guid LoadId { get; set; }
        public Load Load { get; set; } = null!;

        public int Sequence { get; set; }
        public StopType StopType { get; set; } // Pickup / Delivery

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
        // PLANNED (order route snapshot)
        // ======================
        public DateTime? PlannedArrivalFrom { get; set; }
        public DateTime? PlannedArrivalTo { get; set; }

        public DateTime? PlannedDepartureFrom { get; set; }
        public DateTime? PlannedDepartureTo { get; set; }

        public AppointmentType AppointmentType { get; set; } = AppointmentType.Appointment;
        public int? FlexMinutes { get; set; }
        public string TimeZone { get; set; } = "UTC";
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pending;
        public bool AppointmentConfirmed { get; set; }
        public string? AppointmentConfirmationNumber { get; set; }

        // ======================
        // REVISED (dispatcher update)
        // ======================
        public DateTime? RevisedArrivalFrom { get; set; }
        public DateTime? RevisedArrivalTo { get; set; }

        // ======================
        // ACTUAL (execution)
        // ======================
        public DateTime? ActualArrival { get; set; }
        public DateTime? ActualDeparture { get; set; }

        public DateTime? ActualCheckedInTime { get; set; }
        public DateTime? ActualCheckedOutTime { get; set; }

        public bool IsLateArrival { get; set; }
        public bool IsLateDeparture { get; set; }

        public string? AppointmentNumber { get; set; }
        public string StopReference { get; set; }
        public string? PONumbers { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }
        public DateTime? PredictedArrivalAt { get; set; }
        public bool IsAtRiskOfDelay { get; set; }
        public DelayRiskLevel DelayRisk { get; set; } = DelayRiskLevel.None;
        public int? MinutesLatePrediction { get; set; }


        // ======================
        // STATUS (core execution state)
        // ======================
        public StopStatus Status { get; set; } = StopStatus.Pending;

        public string? Notes { get; set; }
        public bool? IsOnTime { get; set; }
        public int? MinutesLate { get; set; }
        public string? DelayRiskReason { get; set; }

        public ICollection<LoadStopServiceRequirement> Services { get; set; } = new List<LoadStopServiceRequirement>();

    }
}
