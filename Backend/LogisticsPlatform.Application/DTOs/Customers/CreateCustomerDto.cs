using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public CustomerBillingDto Billing { get; set; } = new();

    }

}
