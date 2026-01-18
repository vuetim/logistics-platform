using LogisticsPlatform.Application.DTOs.Users;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Repositories.Queries;

public interface IUserQueryRepository
{
    Task<PagedResult<UserListItemDto>> GetPagedAsync(
        UsersQueryParameters parameters,
        Guid currentUserId
    );

    Task<User?> GetByIdAsync(Guid id);
}
