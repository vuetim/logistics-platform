using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/loads/{loadId:guid}/carrier-assignments")]
public class CarrierAssignmentController : ControllerBase
{
    private readonly ICarrierAssignmentService _service;

    public CarrierAssignmentController(ICarrierAssignmentService service)
    {
        _service = service;
    }

    [HttpPost("tender")]
    public async Task<IActionResult> Tender(
        Guid loadId,
        [FromBody] TenderCarrierDto dto)
    {
        dto.LoadId = loadId;
        var id = await _service.TenderAsync(dto, User.GetUserId());
        return Ok(new { assignmentId = id });
    }

    [HttpPost("{assignmentId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid assignmentId)
    {
        await _service.AcceptAsync(assignmentId, User.GetUserId());
        return NoContent();
    }

    [HttpPost("{assignmentId:guid}/reject")]
    public async Task<IActionResult> Reject(Guid assignmentId)
    {
        await _service.RejectAsync(assignmentId, User.GetUserId());
        return NoContent();
    }
}
