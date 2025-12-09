using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class LoadExecutionDetailsDto
{
    public Guid Id { get; set; }
    public string LoadNumber { get; set; } = string.Empty;

    //  Execution truth
    public LoadStatus Status { get; set; }
    public ModeType Mode { get; set; }

    //  Parties
    public string CustomerName { get; set; } = string.Empty;
    public string? CarrierName { get; set; }

    //  Lane
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    //  Derived dates (from stops)
    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? PlannedDeliveryDate { get; set; }
    public DateTime? ActualPickupDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }

    //  Finance
    public decimal? CustomerRate { get; set; }
    public decimal? CarrierRate { get; set; }
    public decimal? Margin => CustomerRate - CarrierRate;

    //  Execution stops
    public IReadOnlyList<LoadStopDetailsDto> Stops { get; set; } = new List<LoadStopDetailsDto>();
    public List<LoadItemDto> Items { get; set; } = new();

}
