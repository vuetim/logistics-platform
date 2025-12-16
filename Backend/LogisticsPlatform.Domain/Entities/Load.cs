using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class Load : BaseEntity
    {
        public string LoadNumber { get; set; } = string.Empty;

        // Relations
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public Guid? CarrierId { get; set; }
        public Carrier? Carrier { get; set; }

        // Status
        public LoadStatus Status { get; set; } = LoadStatus.Draft;

        // Lane (quick UI view)
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

        // Finance (summary)
        public decimal? CustomerRate { get; set; }
        public decimal? CarrierRate { get; set; }
        public decimal? Accessorials { get; set; }
        public ModeType Mode { get; set; } = ModeType.TL;

        // Flags
        public bool IsTemperatureControlled { get; set; }
        public bool IsArchived { get; set; }
        public bool HasEquipment { get; set; }

        // Navigation
        public ICollection<LoadStop> Stops { get; set; } = new List<LoadStop>();
        public ICollection<LoadEquipment> Equipment { get; set; } = new List<LoadEquipment>();
        public ICollection<LoadOrder> Orders { get; set; } = new List<LoadOrder>();
        public ICollection<LoadNote> Notes { get; set; } = new List<LoadNote>();
        public ICollection<LoadDocument> Documents { get; set; } = new List<LoadDocument>();
        public ICollection<LoadItem> Items { get; set; } = new List<LoadItem>();

        //financials 
        public LoadCost? Cost { get; set; }
        public string? BolNumber { get; set; }
        public string? ProNumber { get; set; }
        public string? RateConfirmationNumber { get; set; }
        public string? TrackingNumber { get; set; }

        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? DriverEmail { get; set; }

        public string? TruckNumber { get; set; }
        public string? TrailerNumber { get; set; }
        public string? CarrierSCAC { get; set; }

        public DateTime? PodReceivedAt { get; set; }
        public string? PodUploadedBy { get; set; }

        public bool OnTimePickup { get; set; }
        public bool OnTimeDelivery { get; set; }
        public int? TransitTimeHours { get; set; }
        public bool HasDelayRisk { get; set; }

        public ICollection<LoadCarrierAssignment> CarrierAssignments { get; set; }
    = new List<LoadCarrierAssignment>();

        // Audit
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
