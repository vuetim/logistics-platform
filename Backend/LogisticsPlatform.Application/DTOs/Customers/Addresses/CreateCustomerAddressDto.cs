using LogisticsPlatform.Domain.Enums;

public class CreateCustomerAddressDto
{
    public Guid CustomerId { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public CustomerAddressType Type { get; set; }
    public bool IsPrimary { get; set; }
    public bool isActive { get; set; }
}
