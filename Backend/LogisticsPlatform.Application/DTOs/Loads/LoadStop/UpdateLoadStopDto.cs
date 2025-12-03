using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadStop
{
    public class UpdateLoadStopDto
    {
        public StopType StopType { get; set; }
        public int Sequence { get; set; }

        // Location
        public string LocationName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Planned
        public DateTime? PlannedArrivalFrom { get; set; }
        public DateTime? PlannedArrivalTo { get; set; }

        public DateTime? PlannedDepartureFrom { get; set; }
        public DateTime? PlannedDepartureTo { get; set; }

        public AppointmentType AppointmentType { get; set; } = AppointmentType.Appointment;
        public int? FlexMinutes { get; set; }

        public string? Notes { get; set; }
    }
}
