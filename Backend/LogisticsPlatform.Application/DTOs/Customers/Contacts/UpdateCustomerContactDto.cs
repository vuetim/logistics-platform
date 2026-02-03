namespace LogisticsPlatform.Application.DTOs.Customers.Contacts
{
    public class UpdateCustomerContactDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public bool IsPrimary { get; internal set; }
        public bool IsActive { get; internal set; }
    }
}
