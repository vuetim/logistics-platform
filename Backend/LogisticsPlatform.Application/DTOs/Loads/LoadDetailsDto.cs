using LogisticsPlatform.Domain.Enums;

public class LoadDetailsDto
{
    public Guid Id { get; set; }
    public string LoadNumber { get; set; } = string.Empty;

    public string ReferenceNumber { get; set; } = string.Empty;
    public LoadStatus Status { get; set; }

    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Guid? CarrierId { get; set; }
    public string? CarrierName { get; set; }

    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    public DateTime? PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public ModeType? ModeType { get; set; }

    public decimal CustomerRate { get; set; }
    public decimal CarrierRate { get; set; }
    public decimal? Accessorials { get; set; }
    public decimal? Margin { get; set; }

    public string Summary { get; set; } = string.Empty;

    public List<EquipmentType> EquipmentTypes { get; set; } = new();
}
