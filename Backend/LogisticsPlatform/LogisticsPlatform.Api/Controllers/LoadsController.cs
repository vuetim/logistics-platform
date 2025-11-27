using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Services;
using LogisticsPlatform.Domain.Enums;
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

       
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] LoadQueryParameters parameters)
        {
            var result = await _queries.GetPagedAsync(parameters);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result = await _queries.GetDetailsAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLoadDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var id = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetDetails), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateLoadDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, LoadStatus status)
        {
            await _service.ChangeStatusAsync(id, status);
            return Ok();
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> Archive(Guid id)
        {
            await _service.ArchiveAsync(id);
            return Ok();
        }
    }
}
