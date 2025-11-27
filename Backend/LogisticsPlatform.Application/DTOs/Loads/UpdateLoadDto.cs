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
}
