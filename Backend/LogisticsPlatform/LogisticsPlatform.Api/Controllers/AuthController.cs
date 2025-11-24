using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _auth.RegisterAsync(dto);
            return Ok("Registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _auth.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDto dto)
        {
            try
            {
                await _auth.AssignRoleAsync(dto);
                return Ok("Role assigned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test-admin")]
        public IActionResult TestAdmin()
        {
            return Ok("You are ADMIN!");
        }

    }

}
