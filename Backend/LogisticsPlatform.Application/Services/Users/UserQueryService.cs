using LogisticsPlatform.Application.DTOs.Users;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Application.Interfaces.Services.Users;

namespace LogisticsPlatform.Application.Services.Users;

public class UserQueryService : IUserQueryService
{
    private readonly IUserQueryRepository _repo;

    public UserQueryService(IUserQueryRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<UserListItemDto>> GetPagedAsync(
        UsersQueryParameters parameters,
        Guid currentUserId)
    {
        if (parameters.PageSize > 100)
            parameters.PageSize = 100;

        return await _repo.GetPagedAsync(parameters, currentUserId);
    }

    public async Task<UserDetailsDto?> GetDetailsAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null)
            return null;

        return new UserDetailsDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
        };
    }
}
