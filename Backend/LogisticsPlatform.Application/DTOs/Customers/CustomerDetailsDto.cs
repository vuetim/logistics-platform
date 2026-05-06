using LogisticsPlatform.Application.DTOs.Customers.Addresses;
using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public CustomerBillingDto Billing { get; set; } = new();


        public List<CustomerAddressDto> Addresses { get; set; } = [];
        public List<CustomerContactDto> Contacts { get; set; } = [];
        public List<CustomerNoteDto> Notes { get; set; } = [];
    }
}
