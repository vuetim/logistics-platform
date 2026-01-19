using LogisticsPlatform.Application.DTOs.Customers.Addresses;


namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int PaymentTermsDays { get; set; }

        public List<CustomerAddressDto> Addresses { get; set; } = [];
    }

}
