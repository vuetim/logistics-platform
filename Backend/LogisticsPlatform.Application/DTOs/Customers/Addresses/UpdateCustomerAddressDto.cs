using LogisticsPlatform.Domain.Enums;

public class UpdateCustomerAddressDto
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public CustomerAddressType? Type { get; set; }
    public bool? IsPrimary { get; set; }
    public bool? IsActive { get; internal set; }
}
