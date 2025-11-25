namespace LogisticsPlatform.Application.DTOs.Carriers.Contacts
{
    public class CreateCarrierContactDto
    {
        public Guid CarrierId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
    }
}
