using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.DTOs.Users;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly AppDbContext _context;

    public UserQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    // ==========================
    // LIST VIEW (TABLE)
    // ==========================
    public async Task<PagedResult<UserListItemDto>> GetPagedAsync(
       UsersQueryParameters parameters,
       Guid currentUserId)
    {
        var query = _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
            .Where(u => u.Id != currentUserId);

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var s = parameters.Search.ToLower();
            query = query.Where(u =>
                u.FullName.ToLower().Contains(s) ||
                u.Email.ToLower().Contains(s));
        }

        if (parameters.IsActive.HasValue)
            query = query.Where(u => u.IsActive == parameters.IsActive);

        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            query = parameters.SortDir == "desc"
                ? query.OrderByDescendingDynamic(parameters.SortBy)
                : query.OrderByDynamic(parameters.SortBy);
        }
        if (!string.IsNullOrWhiteSpace(parameters.Role))
        {
            var role = parameters.Role.ToLower();
            query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name.ToLower() == role));
        }

        var total = await query.CountAsync();

        var items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(u => new UserListItemDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();

        return new PagedResult<UserListItemDto>(
            items,
            total,
            parameters.Page,
            parameters.PageSize
        );
    }



    // ==========================
    // DETAILS
    // ==========================
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
