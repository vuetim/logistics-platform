using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Customers.Contacts
{
    public class CreateCustomerContactDto
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = CustomerContactRoles.Other;
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
    }
}
