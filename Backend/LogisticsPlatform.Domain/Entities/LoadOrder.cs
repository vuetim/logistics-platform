using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;

public class LoadOrder : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid LoadId { get; set; }
    public Load Load { get; set; } = null!;

    public string PONumber { get; set; } = string.Empty;
}
