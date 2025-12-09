using LogisticsPlatform.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/loads")]
    [Authorize]
    public class LoadActivityController : ControllerBase
    {
        private readonly IActivityLogQueryRepository _activityQuery;

        public LoadActivityController(IActivityLogQueryRepository activityQuery)
        {
            _activityQuery = activityQuery;
        }

        // GET: /api/loads/{loadId}/activity
        [HttpGet("{loadId:guid}/activity")]
        public async Task<IActionResult> GetActivity(Guid loadId)
        {
            var activity = await _activityQuery
                .GetByEntityAsync("Load", loadId);

            return Ok(activity);
        }
    }
}
