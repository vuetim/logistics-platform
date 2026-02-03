

namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CustomerListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
