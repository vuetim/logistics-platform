using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Security;
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
        private readonly IPermissionService _permissions;

        public LoadStopsExecutionController(ILoadStopExecutionService service, IPermissionService permissions)
        {
            _service = service;
            _permissions = permissions;
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
            if (!await CanUpdateExecutionAsync(userId))
                return Forbid();
            await _service.MarkEnRouteAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/arrive
        [HttpPost("{stopId:guid}/arrive")]
        public async Task<IActionResult> MarkArrived(Guid stopId)
        {
            var userId = GetUserId();
            if (!await CanUpdateExecutionAsync(userId))
                return Forbid();
            await _service.MarkArrivedAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/loaded
        [HttpPost("{stopId:guid}/loaded")]
        public async Task<IActionResult> MarkLoaded(Guid stopId)
        {
            var userId = GetUserId();
            if (!await CanUpdateExecutionAsync(userId))
                return Forbid();
            await _service.MarkLoadedAsync(stopId, userId);
            return NoContent();
        }

        // POST: /api/load-stops/{stopId}/unloaded
        [HttpPost("{stopId:guid}/unloaded")]
        public async Task<IActionResult> MarkUnloaded(Guid stopId)
        {
            var userId = GetUserId();
            if (!await CanUpdateExecutionAsync(userId))
                return Forbid();
            await _service.MarkUnloadedAsync(stopId, userId);
            return NoContent();
        }

        private async Task<bool> CanUpdateExecutionAsync(Guid userId)
        {
            return await _permissions.HasPermissionAsync(userId, Permission.Load_Tracking_Update)
                || await _permissions.HasPermissionAsync(userId, Permission.Load_ChangeStatus);
        }
    }
}
