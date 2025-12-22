using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
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

    public UsersController(IUserManagementService users)
    {
        _users = users;
    }

    // GET /api/users  (Admin)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var currentUserId = GetCurrentUserId();
        return Ok(await _users.GetAllAsync(currentUserId));
    }

    // GET /api/users/{id}  (Admin ose vetvetja)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var user = await _users.GetByIdAsync(id, currentUserId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // PUT /api/users/{id}  (Admin ose vetvetja)
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

    // 🔹 helper privat
    private Guid GetCurrentUserId()
    {
        return Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }
}
