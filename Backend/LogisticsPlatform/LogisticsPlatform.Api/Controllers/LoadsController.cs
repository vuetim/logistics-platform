using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Services;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/loads")]
    [Authorize]
    public class LoadsController : ControllerBase
    {
        private readonly ILoadService _service;
        private readonly ILoadQueryService _queries;

        public LoadsController(
            ILoadService service,
            ILoadQueryService queries)
        {
            _service = service;
            _queries = queries;
        }

        //  GET LIST (permission check in QueryService)
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] LoadQueryParameters parameters)
        {
            var result = await _queries.GetPagedAsync(parameters);
            return Ok(result);
        }

        //  GET DETAILS
        [HttpGet("{id}", Name = "GetLoadDetails")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result = await _queries.GetDetailsAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        //  CREATE LOAD
        [HttpPost]
        public async Task<IActionResult> Create(CreateLoadDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var id = await _service.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetDetails), new { id }, null);
        }

        //  UPDATE LOAD
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateLoadDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.UpdateAsync(id, dto, userId);

            return Ok();
        }

        //  CHANGE STATUS
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, LoadStatus status)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.ChangeStatusAsync(id, status, userId);

            return Ok();
        }

        //dispatch 
        [HttpPost("{loadId:guid}/dispatch")]
        public async Task<IActionResult> Dispatch(
    Guid loadId,
    [FromBody] DispatchLoadDto dto)
        {
            await _service.DispatchAsync(loadId, dto, User.GetUserId());
            return NoContent();
        }


        //  ARCHIVE LOAD
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.ArchiveAsync(id, userId);

            return Ok();
        }

    }
}
