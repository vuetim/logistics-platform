using LogisticsPlatform.Application.DTOs.Auth;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Interfaces.Services.Auth;
using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IAuthAuditService _audit;


        public AuthController(IAuthService auth, IAuthAuditService audit)
        {
            _auth = auth;
            _audit = audit;
        }

        [HttpPost("login")]
        [AllowAnonymous]
    
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _auth.LoginAsync(dto);

                await _audit.LogAsync(
                    result.UserId,
                    "Auth.Login.Success",
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Request.Headers["User-Agent"].ToString()
                );

                return Ok(result);
            }
            catch
            {
                await _audit.LogAsync(
                    null,
                    "Auth.Login.Failed",
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Request.Headers["User-Agent"].ToString()
                );

                throw;
            }
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
       

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _auth.LogoutAsync(dto.RefreshToken);

            await _audit.LogAsync(
                userId,
                "Auth.Logout",
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers["User-Agent"].ToString()
            );

            return Ok();
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var userId = await _auth.ForgotPasswordAsync(dto.Email);

            if (userId.HasValue)
            {
                await _audit.LogAsync(
                    userId.Value,
                    "Auth.Password.Reset.Requested",
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Request.Headers["User-Agent"].ToString()
                );
            }

            return Ok(); 
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var result = await _auth.RefreshAsync(dto.RefreshToken);

            await _audit.LogAsync(
                result.UserId,
                "Auth.Token.Refreshed",
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers["User-Agent"].ToString()
            );

            return Ok(result);
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var userId = await _auth.ResetPasswordAsync(dto.Token, dto.NewPassword);

            await _audit.LogAsync(
                userId,
                "Auth.Password.Reset.Completed",
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers["User-Agent"].ToString()
            );

            return Ok();
        }



    }
}
