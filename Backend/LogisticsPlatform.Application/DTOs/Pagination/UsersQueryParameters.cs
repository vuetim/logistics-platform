

namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class UsersQueryParameters : QueryParameters
    {
        public bool? IsActive { get; set; }
        public string? Role { get; set; }
    }

}