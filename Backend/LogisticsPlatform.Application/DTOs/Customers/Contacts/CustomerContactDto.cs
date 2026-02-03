
namespace LogisticsPlatform.Application.DTOs.Customers.Contacts {
    public class CustomerContactDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Position { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }

    }


}