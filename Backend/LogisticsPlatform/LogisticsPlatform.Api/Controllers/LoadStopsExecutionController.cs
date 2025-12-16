using LogisticsPlatform.Application.Interfaces.Services.Loads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/load-stops")]
    [Authorize]
    public class LoadStopsExecutionController : ControllerBase
    {
        private readonly ILoadStopExecutionService _service;

        public LoadStopsExecutionController(ILoadStopExecutionService service)
        {
            _service = service;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("User id not found in token.");

            return Guid.Parse(id);
        }

        // POST: /api/load-stops/{stopId}/enroute
        [HttpPost("{stopId:guid}/enroute")]
        public async Task<IActionResult> MarkEnRoute(Guid stopId)
        {
            var userId = GetUserId();
            await _service.MarkEnRouteAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/arrive
        [HttpPost("{stopId:guid}/arrive")]
        public async Task<IActionResult> MarkArrived(Guid stopId)
        {
            var userId = GetUserId();
            await _service.MarkArrivedAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/loaded
        [HttpPost("{stopId:guid}/loaded")]
        public async Task<IActionResult> MarkLoaded(Guid stopId)
        {
            var userId = GetUserId();
            await _service.MarkLoadedAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/unloaded
        [HttpPost("{stopId:guid}/unloaded")]
        public async Task<IActionResult> MarkUnloaded(Guid stopId)
        {
            var userId = GetUserId();
            await _service.MarkUnloadedAsync(stopId, userId);
            return NoContent();
        }
    }
}
