namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class UpdateCustomerDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int PaymentTermsDays { get; set; } = 30; // NET 30 default
        public bool? IsActive { get; set; }

    }
}
