using System.ComponentModel.DataAnnotations;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads;

public class UpdateLoadDto
{
    public Guid? CarrierId { get; set; }

    public ModeType? ModeType { get; set; }
    public EquipmentType? EquipmentType { get; set; }

    public DateTime? PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    [Range(0, 999999999)]
    public decimal? CustomerRate { get; set; }

    [Range(0, 999999999)]
    public decimal? CarrierRate { get; set; }

    [Range(0, 999999999)]
    public decimal? Accessorials { get; set; }

    [MaxLength(500)]
    public string? Summary { get; set; }

    [MaxLength(250)]
    public string? Origin { get; set; }

    [MaxLength(250)]
    public string? Destination { get; set; }

    [MaxLength(80)]
    public string? BolNumber { get; set; }

    [MaxLength(80)]
    public string? ProNumber { get; set; }

    [MaxLength(80)]
    public string? RateConfirmationNumber { get; set; }

    [MaxLength(120)]
    public string? TrackingNumber { get; set; }

    [MaxLength(120)]
    public string? DriverName { get; set; }

    [MaxLength(40)]
    public string? DriverPhone { get; set; }

    [EmailAddress]
    [MaxLength(160)]
    public string? DriverEmail { get; set; }

    [MaxLength(40)]
    public string? TruckNumber { get; set; }

    [MaxLength(40)]
    public string? TrailerNumber { get; set; }

    [MaxLength(10)]
    public string? CarrierSCAC { get; set; }

    public DateTime? PodReceivedAt { get; set; }

    [MaxLength(120)]
    public string? PodUploadedBy { get; set; }
}
