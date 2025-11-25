namespace LogisticsPlatform.Application.DTOs.Carriers.Addresses
{
    public class UpdateCarrierAddressDto
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Type { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
