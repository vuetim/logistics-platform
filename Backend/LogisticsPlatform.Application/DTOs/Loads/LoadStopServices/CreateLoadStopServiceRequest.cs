namespace LogisticsPlatform.Application.DTOs.Loads.LoadStopServices;

public class CreateLoadStopServiceRequest
{
    public string ServiceKey { get; set; } = string.Empty;
    public string ServiceValue { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsPickupService { get; set; }
    public bool IsDeliveryService { get; set; }
}
