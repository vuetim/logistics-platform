using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/ui/table-preferences")]
    public class UserTablePreferencesController : ControllerBase
    {
        private readonly IUserTablePreferenceService _service;

        public UserTablePreferencesController(IUserTablePreferenceService service)
        {
            _service = service;
        }

        [HttpGet("{tableKey}")]
        public async Task<IActionResult> Get(string tableKey)
        {
            var userId = User.GetUserId();
            var result = await _service.GetAsync(userId, tableKey);
            return Ok(result);
        }

        [HttpPut("{tableKey}")]
        public async Task<IActionResult> Put(string tableKey, [FromBody] UpdateUserTablePreferenceDto dto)
        {
            var userId = User.GetUserId();
            var result = await _service.UpsertAsync(userId, tableKey, dto);
            return Ok(result);
        }
    }
}
