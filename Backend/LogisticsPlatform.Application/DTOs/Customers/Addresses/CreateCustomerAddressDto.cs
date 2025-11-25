public class CreateCustomerAddressDto
{
    public Guid CustomerId { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string Type { get; set; } = "Shipping";
    public bool IsPrimary { get; set; }
}
