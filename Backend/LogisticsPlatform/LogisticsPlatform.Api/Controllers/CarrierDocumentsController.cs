using LogisticsPlatform.Application.DTOs.Carriers.Documents;
using LogisticsPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CarrierDocumentsController : ControllerBase
    {
        private readonly ICarrierDocumentService _service;

        public CarrierDocumentsController(ICarrierDocumentService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("carrier/{carrierId}")]
        public async Task<IActionResult> GetByCarrier(Guid carrierId)
        {
            var docs = await _service.GetByCarrierAsync(carrierId);
            return Ok(docs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarrierDocumentDto dto)
        {
            var userId = GetUserId();
            var result = await _service.CreateAsync(userId, dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCarrierDocumentDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? Ok("Deleted") : NotFound();
        }
    }
}
