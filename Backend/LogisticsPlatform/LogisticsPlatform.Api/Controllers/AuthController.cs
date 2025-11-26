using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _auth.LoginAsync(dto);
            return Ok(new { token });
        }

        [HttpPost("register")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _auth.RegisterAsync(dto);
            return Ok();
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> AssignRole(AssignRoleDto dto)
        {
            await _auth.AssignRoleAsync(dto);
            return Ok();
        }
        [HttpGet("users")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _auth.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("users/{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _auth.GetUserByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("users/role/{roleName}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var result = await _auth.GetUsersByRoleAsync(roleName);
            return Ok(result);
        }
        [HttpPut("users/{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            await _auth.UpdateUserAsync(id, dto);
            return Ok("User updated");
        }
        [HttpDelete("users/{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _auth.DeleteUserAsync(id);
            return Ok("User deleted");
        }


    }
}
