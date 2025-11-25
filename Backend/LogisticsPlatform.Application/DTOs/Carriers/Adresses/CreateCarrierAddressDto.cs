namespace LogisticsPlatform.Application.DTOs.Carriers.Addresses
{
    public class CreateCarrierAddressDto
    {
        public Guid CarrierId { get; set; }

        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public string Type { get; set; } = "General";
        public bool IsPrimary { get; set; } = false;
    }
}
