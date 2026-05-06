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
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid? CarrierId { get; set; }
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

    public decimal? Accessorials { get; set; }
    public string? BolNumber { get; set; }
    public string? ProNumber { get; set; }
    public string? RateConfirmationNumber { get; set; }
    public string? TrackingNumber { get; set; }

    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public string? DriverEmail { get; set; }

    public string? TruckNumber { get; set; }
    public string? TrailerNumber { get; set; }
    public string? CarrierSCAC { get; set; }

    public DateTime? PodReceivedAt { get; set; }
    public string? PodUploadedBy { get; set; }

    //  Execution stops
    public IReadOnlyList<LoadStopDetailsDto> Stops { get; set; } = new List<LoadStopDetailsDto>();
    public List<LoadItemDto> Items { get; set; } = new();

}
