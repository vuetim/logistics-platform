using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads;

public class UpdateLoadDto
{
    public Guid? CarrierId { get; set; }

    public ModeType? ModeType { get; set; }
    public EquipmentType? EquipmentType { get; set; }

    public DateTime? PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public decimal? CustomerRate { get; set; }
    public decimal? CarrierRate { get; set; }
    public decimal? Accessorials { get; set; }

    public string? Summary { get; set; }
    public string? Origin { get; internal set; }
    public string? Destination { get; internal set; }
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
}
