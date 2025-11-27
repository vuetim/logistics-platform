using LogisticsPlatform.Application.DTOs.Loads.LoadDocuments;
using LogisticsPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers;

[ApiController]
[Route("api/loads/{loadId:guid}/documents")]
[Authorize]
public class LoadDocumentsController : ControllerBase
{
    private readonly ILoadDocumentService _service;

    public LoadDocumentsController(ILoadDocumentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid loadId, CreateLoadDocumentDto dto)
    {
        await _service.AddAsync(loadId, dto);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetByLoad(Guid loadId)
    {
        var docs = await _service.GetByLoadAsync(loadId);
        return Ok(docs);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
