namespace LogisticsPlatform.Application.DTOs.Common
{
    public class UserTablePreferenceDto
    {
        public string TableKey { get; set; } = string.Empty;
        public string JsonConfig { get; set; } = "{}";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
