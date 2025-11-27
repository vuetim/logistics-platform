using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Pagination;

public class LoadQueryParameters : QueryParameters
{
    public LoadStatus? Status { get; set; }

    public Guid? CustomerId { get; set; }
    public Guid? CarrierId { get; set; }

    public ModeType? Mode { get; set; }

    public DateTime? PickupFrom { get; set; }
    public DateTime? PickupTo { get; set; }

    public DateTime? DeliveryFrom { get; set; }
    public DateTime? DeliveryTo { get; set; }
}
