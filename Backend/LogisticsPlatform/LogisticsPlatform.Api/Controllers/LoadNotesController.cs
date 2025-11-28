using LogisticsPlatform.Application.DTOs.Loads.LoadNote;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/loads/{loadId}/notes")]
public class LoadNotesController : ControllerBase
{
    private readonly ILoadNoteService _service;

    public LoadNotesController(ILoadNoteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid loadId)
        => Ok(await _service.GetByLoadAsync(loadId));

    [HttpPost]
    public async Task<IActionResult> Create(Guid loadId, CreateLoadNoteDto dto)
    {
        var userId = User.GetUserId(); // extension
        await _service.AddAsync(loadId, dto, userId);
        return Ok();
    }
}
