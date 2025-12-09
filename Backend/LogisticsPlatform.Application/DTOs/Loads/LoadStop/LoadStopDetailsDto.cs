using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadStop
{
    public class LoadStopDetailsDto
    {
        // ======================
        // CORE
        // ======================
        public Guid Id { get; set; }

        public int Sequence { get; set; }
        public StopType StopType { get; set; }          // Pickup / Delivery
        public StopStatus Status { get; set; }          // Pending / Arrived / Completed

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

        // ======================
        // PLANNED (from OrderRoute snapshot)
        // ======================
        public DateTime? PlannedArrivalFrom { get; set; }
        public DateTime? PlannedArrivalTo { get; set; }

        public AppointmentType AppointmentType { get; set; }
        public int? FlexMinutes { get; set; }

        // ======================
        // REVISED (dispatcher updates)
        // ======================
        public DateTime? RevisedArrivalFrom { get; set; }
        public DateTime? RevisedArrivalTo { get; set; }

        // ======================
        // ACTUAL (execution truth)
        // ======================
        public DateTime? ActualArrival { get; set; }
        public DateTime? ActualDeparture { get; set; }

        // ======================
        // META
        // ======================
        public string? Notes { get; set; }
    }
}
