using LogisticsPlatform.Api.Controllers.LoadDocuments;
using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/loads/{loadId:guid}/documents")]
[Authorize]
public class LoadDocumentsController : ControllerBase
{
    private readonly ILoadDocumentService _service;
    private readonly IWebHostEnvironment _env;

    public LoadDocumentsController(
        ILoadDocumentService service,
        IWebHostEnvironment env)
    {
        _service = service;
        _env = env;
    }

    // ✅ UPLOAD FILE REAL
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        Guid loadId,
        [FromForm] UploadLoadDocumentRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is required");

        var uploadsRoot = Path.Combine(
            _env.WebRootPath,
            "uploads",
            "loads",
            loadId.ToString()
        );

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var safeFileName = $"{Guid.NewGuid()}_{request.File.FileName}";
        var fullPath = Path.Combine(uploadsRoot, safeFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var fileUrl = $"/uploads/loads/{loadId}/{safeFileName}";

        await _service.AddAsync(loadId, new CreateLoadDocumentDto
        {
            DocumentType = request.DocumentType,
            FileUrl = fileUrl
        });

        return Created(fileUrl, new
        {
            documentType = request.DocumentType,
            fileUrl
        });
    }

    // ✅ LIST DOCUMENTS
    [HttpGet]
    public async Task<IActionResult> GetByLoad(Guid loadId)
    {
        var docs = await _service.GetByLoadAsync(loadId);
        return Ok(docs);
    }

    // DELETE DOCUMENT
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
