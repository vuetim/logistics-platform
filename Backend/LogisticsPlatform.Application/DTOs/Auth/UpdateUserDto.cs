namespace LogisticsPlatform.Application.DTOs.Auth
{
    public class UpdateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? NewPassword { get; set; }
    }
}
