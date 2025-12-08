using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities;

public class LoadCost : BaseEntity
{
    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }   // total payable për carrier

    public ICollection<LoadCostLineItem> LineItems { get; set; } = new List<LoadCostLineItem>();
}