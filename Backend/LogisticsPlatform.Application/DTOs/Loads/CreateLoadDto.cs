using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Loads;

public class CreateLoadDto
{
    public Guid CustomerId { get; set; }
    public Guid? CarrierId { get; set; }

    public ModeType ShipmentType { get; set; }       // FTL / LTL
    public EquipmentType EquipmentType { get; set; }     // DryVan, Reefer, Flatbed
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime PickupDate { get; set; }
    public DateTime DeliveryDate { get; set; }

    public decimal CustomerRate { get; set; }
    public decimal CarrierRate { get; set; }
    public decimal? Accessorials { get; set; }

    public string? Summary { get; set; }
    public bool IsTemperatureControlled { get; set; }
}
