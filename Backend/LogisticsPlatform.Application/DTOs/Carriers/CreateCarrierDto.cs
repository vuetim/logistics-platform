namespace LogisticsPlatform.Application.DTOs.Carriers
{
    public class CreateCarrierDto
    {
        public string Name { get; set; } = string.Empty;
        public string McNumber { get; set; } = string.Empty;
        public string DotNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // "Active", "Inactive", "Blocked"
        public string Status { get; set; } = "Active";
        public int Rating { get; set; } = 0;
    }
}
