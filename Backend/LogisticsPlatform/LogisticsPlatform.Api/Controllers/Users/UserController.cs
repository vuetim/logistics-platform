using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService _users;
    private readonly IUserQueryService _userQueryService;
    public UsersController(IUserManagementService users, IUserQueryService userQueryService
)
    {
        _users = users;
        _userQueryService = userQueryService;


    }

    // GET /api/users  (Admin)
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] UsersQueryParameters parameters)
    {
        var currentUserId = GetCurrentUserId();

        var result = await _userQueryService.GetPagedAsync(
            parameters,
            currentUserId
        );

        return Ok(result);
    }




    // GET /api/users/{id}  
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var user = await _users.GetByIdAsync(id, currentUserId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // PUT /api/users/{id}  
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var currentUserId = GetCurrentUserId();
        await _users.UpdateAsync(id, dto, currentUserId);
        return Ok();
    }

    // DELETE /api/users/{id}  (Admin)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        await _users.DeleteAsync(id, currentUserId);
        return Ok();
    }
    //assign role
    [HttpPost("assign-role")]
    [Authorize]
    public async Task<IActionResult> AssignRole(
    AssignRoleDto dto)
    {
        var currentUserId =
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _users.AssignRoleAsync(dto, currentUserId);
        return Ok();
    }
    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> SetPermission(
     Guid id,
     [FromBody] SetUserPermissionDto dto)
    {
        var currentUserId = GetCurrentUserId();

        await _users.SetPermissionAsync(
            id,
            dto.Permission,
            dto.IsAllowed,
            currentUserId);

        return Ok();
    }
    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id)
    {
        var currentUserId = GetCurrentUserId();

        var perms = await _users.GetPermissionsAsync(id, currentUserId);
        return Ok(perms);
    }





    // helper privat
    private Guid GetCurrentUserId()
    {
        return Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }
}
