using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerNotesController : ControllerBase
    {
        private readonly ICustomerNoteService _service;

        public CustomerNotesController(ICustomerNoteService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var notes = await _service.GetByCustomerAsync(customerId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerNoteDto dto)
        {
            var userId = GetUserId();
            var note = await _service.CreateAsync(dto, userId);
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerNoteDto dto)
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
