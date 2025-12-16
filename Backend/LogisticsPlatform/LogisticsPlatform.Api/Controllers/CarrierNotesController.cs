using LogisticsPlatform.Application.DTOs.Carriers.Notes;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CarrierNotesController : ControllerBase
    {
        private readonly ICarrierNoteService _service;

        public CarrierNotesController(ICarrierNoteService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("carrier/{carrierId}")]
        public async Task<IActionResult> GetByCarrier(Guid carrierId)
        {
            var notes = await _service.GetByCarrierAsync(carrierId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarrierNoteDto dto)
        {
            var userId = GetUserId();
            var note = await _service.CreateAsync(userId, dto);
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCarrierNoteDto dto)
        {
            var note = await _service.UpdateAsync(id, dto);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? Ok("Deleted") : NotFound();
        }
    }
}
