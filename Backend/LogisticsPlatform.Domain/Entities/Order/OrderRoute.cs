using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class OrderRoute : BaseEntity
    {
        //  Aggregate root
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        //  Order of stops (0 = first pickup)
        public int Sequence { get; set; }

        //  Stop type
        public StopType StopType { get; set; }   // Pickup / Delivery / Stop

        //  Location snapshot (DENORMALIZED for performance)
        public string LocationName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        //  Appointment / Planning
        public DateTime? PlannedArrivalFrom { get; set; }
        public DateTime? PlannedArrivalTo { get; set; }

        public DateTime? PlannedDepartureFrom { get; set; }
        public DateTime? PlannedDepartureTo { get; set; }

        public AppointmentType AppointmentType { get; set; } = AppointmentType.Appointment;
        public int? FlexMinutes { get; set; }
        public bool HasTime { get; set; } = true;
        public string TimeZone { get; set; } = "UTC";
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pending;
        public bool AppointmentConfirmed { get; set; }
        public string? AppointmentConfirmationNumber { get; set; }
        public string? StopReference { get; set; }      // pickup / delivery ref
        public string? AppointmentNumber { get; set; }  // known at planning

        //  Order-specific metadata
        public string? PONumbers { get; set; }
        public string? Notes { get; set; }

        //  Flags for Load creation
        public bool CopyToLoad { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
