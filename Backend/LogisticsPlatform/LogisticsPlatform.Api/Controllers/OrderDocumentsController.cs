using LogisticsPlatform.Api.Controllers.Orders;
using LogisticsPlatform.Application.DTOs.Orders.Documents;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders/{orderId:guid}/documents")]
    public class OrderDocumentsController : ControllerBase
    {
        private readonly IOrderDocumentService _service;
        private readonly IWebHostEnvironment _env;

        public OrderDocumentsController(
            IOrderDocumentService service,
            IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var userId = User.GetUserId();
            var docs = await _service.GetByOrderAsync(orderId, userId);
            return Ok(docs);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(Guid orderId, [FromForm] UploadOrderDocumentRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var uploadsRoot = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "orders",
                orderId.ToString()
            );

            Directory.CreateDirectory(uploadsRoot);

            var safeFileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var fullPath = Path.Combine(uploadsRoot, safeFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await request.File.CopyToAsync(stream);

            var fileUrl = $"/uploads/orders/{orderId}/{safeFileName}";
            var userId = User.GetUserId();

            var created = await _service.CreateAsync(
                orderId,
                new CreateOrderDocumentDto
                {
                    DocumentType = request.DocumentType,
                    FileUrl = fileUrl,
                    IsInternal = request.IsInternal,
                    CopyToLoad = request.CopyToLoad
                },
                userId
            );

            return Ok(created);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var deleted = await _service.DeleteAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
