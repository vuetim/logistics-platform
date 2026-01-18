using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Entities.Security;
using LogisticsPlatform.Domain.Security;


namespace LogisticsPlatform.Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _users;
    private readonly IPermissionService _permissions;
    private readonly IUserRoleRepository _userRoles;
    private readonly IRoleRepository _roles;
    private readonly IAuthAuditService _audit;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;



    public UserManagementService(
        IUserRepository users,
        IPermissionService permissions,
        IUserRoleRepository userRoles,
        IRoleRepository roles,
        IAuthAuditService audit,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher
        )
    {
        _users = users;
        _permissions = permissions;
        _userRoles = userRoles;
        _roles = roles;
        _audit = audit;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;

    }

    //TODO: USER WITH PERMISION USER UPDATE CAN EDIT USER DETALS BUT CANT DISABLE USER THATS JUST FOR ADMIN

    // -----------------------------
    // Helpers
    // -----------------------------
    private static bool IsAdmin(User u)
        => u.UserRoles?.Any(r => r.Role.Name == "Admin") == true;

    private static bool TargetIsAdmin(User target)
        => target.UserRoles?.Any(r => r.Role.Name == "Admin") == true;

    private async Task<User> GetActor(Guid currentUserId)
        => await _users.GetByIdAsync(currentUserId)
           ?? throw new Exception("User not found");

    private async Task<User> GetTarget(Guid id)
        => await _users.GetByIdAsync(id)
           ?? throw new Exception("User not found");

    private static void ForbidIfTargetAdmin(User actor, User target)
    {
        if (TargetIsAdmin(target) && !IsAdmin(actor))
            throw new Exception("Forbidden");
    }

    // -----------------------------
    // GET ALL (Admin style)
    // -----------------------------
    public async Task<List<UserDto>> GetAllAsync(Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_View_All))
            throw new Exception("Forbidden");

        var users = await _users.GetAllAsync();

        // zakonisht s’ka nevojë me pa veten në listë
        users = users.Where(u => u.Id != currentUserId).ToList();

        return users.Select(MapToDto).ToList();
    }

    // -----------------------------
    // GET BY ID
    // Self: needs User_View_Self
    // Others: needs User_View_All
    // -----------------------------
    public async Task<UserDto?> GetByIdAsync(Guid id, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        var target = await _users.GetByIdAsync(id);
        if (target == null) return null;

        if (id == currentUserId)
        {
            // asnjë permission check
        }
        //  OTHER USERS
        else
        {
            if (!await _permissions.HasPermissionAsync(
                currentUserId, Permission.User_View_All))
                throw new Exception("Forbidden");
        }

        

      

        return MapToDto(target);
    }

    // -----------------------------
    // UPDATE
    // Rules:
    // - Needs User_Update always
    // - Non-admin can update ONLY self, unless also has User_View_All (meaning: can manage others)
    // - Nobody non-admin can update Admin
    // - Nobody can disable himself
    // -----------------------------
    public async Task UpdateAsync(Guid id, UpdateUserDto dto, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);
        var target = await GetTarget(id);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_Update))
            throw new Exception("Forbidden");

        var isSelf = (actor.Id == target.Id);

        // non-admin - only self unless has User_View_All
        if (!IsAdmin(actor) && !isSelf)
        {
            var canManageOthers = await _permissions.HasPermissionAsync(currentUserId, Permission.User_View_All);
            if (!canManageOthers)
                throw new Exception("Forbidden");
        }

        // cannot touch admin unless you are admin
        ForbidIfTargetAdmin(actor, target);

        // cannot disable self
        if (isSelf && dto.IsActive == false)
            throw new Exception("You cannot disable yourself");

        target.FullName = dto.FullName;
        target.Email = dto.Email;
        target.IsActive = dto.IsActive;
        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            target.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
            await _refreshTokens.RevokeAllForUserAsync(target.Id);
        }

        await _users.SaveChangesAsync();

        await _audit.LogAsync(
            actor.Id,
            "User.Updated",
            target.Id,
            new { Fields = new[] { "FullName", "Email", "IsActive", dto.NewPassword != null ? "Password" : null }.Where(x => x != null) }
        );
    }

    // -----------------------------
    // ASSIGN ROLE
    // - Needs User_AssignRole
    // - Cannot change own role
    // - Non admin cannot change Admin
    // -----------------------------
    public async Task AssignRoleAsync(AssignRoleDto dto, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_AssignRole))
            throw new Exception("Forbidden");

        var target = await GetTarget(dto.UserId);

        if (actor.Id == target.Id)
            throw new Exception("You cannot change your own role");

        ForbidIfTargetAdmin(actor, target);

        var role = await _roles.GetByNameAsync(dto.RoleName)
            ?? throw new Exception("Role not found");

        if (target.UserRoles != null && target.UserRoles.Any())
            await _userRoles.RemoveRangeAsync(target.UserRoles);

        await _userRoles.AddAsync(new UserRole
        {
            UserId = target.Id,
            RoleId = role.Id
        });

        await _users.SaveChangesAsync();

        await _audit.LogAsync(actor.Id, "User.Role.Assigned", target.Id, new { Role = dto.RoleName });
    }

    // -----------------------------
    // SET PERMISSION OVERRIDE
    // - Needs User_AssignRole (ose krijo Permission.User_SetPermission nëse don ma pastër)
    // - Non admin cannot change Admin
    // -----------------------------
    public async Task SetPermissionAsync(Guid targetUserId, Permission permission, bool? isAllowed, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_AssignRole))
            throw new Exception("Forbidden");

        var target = await GetTarget(targetUserId);

        ForbidIfTargetAdmin(actor, target);

        target.UserPermissions ??= new List<UserPermission>();

        var existing = target.UserPermissions.FirstOrDefault(p => p.Permission == permission);

        if (isAllowed == null)
        {
            if (existing != null)
                target.UserPermissions.Remove(existing);
        }
        else
        {
            if (existing == null)
            {
                target.UserPermissions.Add(new UserPermission
                {
                    UserId = target.Id,
                    Permission = permission,
                    IsAllowed = isAllowed.Value
                });
            }
            else
            {
                existing.IsAllowed = isAllowed.Value;
            }
        }

        await _users.SaveChangesAsync();

        await _audit.LogAsync(actor.Id, "User.Permission.Changed", target.Id,
            new { Permission = permission.ToString(), Allowed = isAllowed });
    }

    // -----------------------------
    // GET PERMISSIONS (state for UI)
    // - Needs User_View_All
    // - Non admin cannot view Admin permission overrides
    // -----------------------------
    public async Task<List<UserPermissionStateDto>> GetPermissionsAsync(Guid targetUserId, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_View_All))
            throw new Exception("Forbidden");

        var target = await GetTarget(targetUserId);

        ForbidIfTargetAdmin(actor, target);

        var overrides = target.UserPermissions == null
            ? new Dictionary<Permission, bool?>()
            : target.UserPermissions.ToDictionary(p => p.Permission, p => (bool?)p.IsAllowed);

        return Enum.GetValues<Permission>()
            .Select(p => new UserPermissionStateDto
            {
                Permission = p,
                IsAllowed = overrides.ContainsKey(p) ? overrides[p] : null
            })
            .ToList();
    }

    // -----------------------------
    // DISABLE
    // - Needs User_Disable
    // - Cannot disable self
    // - Non admin cannot disable Admin
    // -----------------------------
    public async Task DisableAsync(Guid userId, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_Disable))
            throw new Exception("Forbidden");

        var target = await GetTarget(userId);

        if (actor.Id == target.Id)
            throw new Exception("You cannot disable yourself");

        ForbidIfTargetAdmin(actor, target);

        target.IsActive = false;

        await _users.SaveChangesAsync();
        await _audit.LogAsync(actor.Id, "User.Disabled", target.Id);
    }

    // -----------------------------
    // DELETE
    // - Needs User_Delete
    // - Non admin cannot delete Admin
    // -----------------------------
    public async Task DeleteAsync(Guid id, Guid currentUserId)
    {
        var actor = await GetActor(currentUserId);

        if (!await _permissions.HasPermissionAsync(currentUserId, Permission.User_Delete))
            throw new Exception("Forbidden");

        var target = await GetTarget(id);

        ForbidIfTargetAdmin(actor, target);

        await _users.DeleteAsync(target);
        await _users.SaveChangesAsync();
    }

    private static UserDto MapToDto(User u) => new UserDto
    {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email,
        IsActive = u.IsActive,
        Roles = u.UserRoles?.Select(r => r.Role.Name).ToList() ?? new()
    };
}
