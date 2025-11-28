using System.Security.Claims;

namespace LogisticsPlatform.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return id != null
                ? Guid.Parse(id)
                : throw new UnauthorizedAccessException("UserId claim not found");
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role claim not found");
        }
    }
}
