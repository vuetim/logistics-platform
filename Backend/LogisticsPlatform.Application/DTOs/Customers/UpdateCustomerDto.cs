using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class UpdateCustomerDto
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public CustomerBillingDto Billing { get; set; } = new();


        public bool IsActive { get; set; }
    }

}
