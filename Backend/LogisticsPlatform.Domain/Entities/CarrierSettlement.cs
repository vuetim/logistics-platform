using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities.Financial;

public class CarrierSettlement
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public Guid CarrierId { get; set; }
    public Carrier Carrier { get; set; } = null!;

    public string SettlementNumber { get; set; } = string.Empty;
    public DateTime SettlementDate { get; set; }

    public SettlementStatus Status { get; set; } = SettlementStatus.Draft;

    public decimal TotalAmount { get; set; }
    public string? PdfUrl { get; set; }
    //  Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    // PDF link – kur ta gjenerojmë e ruajmë

    public List<CarrierSettlementLineItem> LineItems { get; set; } = new();
    public string Notes { get; set; }
    public DateTime DueDate { get; set; }
}
