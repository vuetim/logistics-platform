namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int PaymentTermsDays { get; set; } = 30; // NET 30 default
        public bool IsActive { get; set; } = true;

    }
}
