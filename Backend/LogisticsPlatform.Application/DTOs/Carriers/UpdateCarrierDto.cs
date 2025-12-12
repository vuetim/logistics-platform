namespace LogisticsPlatform.Application.DTOs.Carriers
{
    public class UpdateCarrierDto
    {
        public string? Name { get; set; }
        public string? McNumber { get; set; }
        public string? DotNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public int PaymentTermsDays { get; set; } = 30; // NET 30 default

        public int? Rating { get; set; }
    }
}
