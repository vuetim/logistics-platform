using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities;

public class LoadStopServiceRequirement : BaseEntity
{
    public Guid LoadStopId { get; set; }
    public LoadStop LoadStop { get; set; } = null!;

    public string ServiceKey { get; set; } = string.Empty;
    public string ServiceValue { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsPickupService { get; set; }
    public bool IsDeliveryService { get; set; }
}
