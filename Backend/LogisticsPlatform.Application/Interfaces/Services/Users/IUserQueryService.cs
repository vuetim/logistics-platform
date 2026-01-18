using LogisticsPlatform.Application.DTOs.Users;
using LogisticsPlatform.Application.DTOs.Pagination;

namespace LogisticsPlatform.Application.Interfaces.Services.Users;

public interface IUserQueryService
{
    Task<PagedResult<UserListItemDto>> GetPagedAsync(
        UsersQueryParameters parameters,
        Guid currentUserId
    );

    Task<UserDetailsDto?> GetDetailsAsync(Guid id);
}
