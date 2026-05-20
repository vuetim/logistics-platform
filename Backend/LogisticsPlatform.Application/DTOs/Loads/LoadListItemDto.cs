using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads;

public class LoadListItemDto
{
    public Guid Id { get; set; }
    public string LoadNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;
    public string? CarrierName { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    public LoadStatus Status { get; set; }
    public ModeType ModeType { get; set; }

    public DateTime? PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int PickupStops { get; set; }
    public int DeliveryStops { get; set; }

    public decimal? CustomerRate { get; set; }
    public decimal? CarrierRate { get; set; }
    public decimal TotalBillable { get; set; }
    public decimal TotalPayable { get; set; }
    public decimal? Margin { get; set; }
    public bool HasEquipment { get; set; }

}
