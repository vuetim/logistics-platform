using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Security;
using LogisticsPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/loads/{loadId:guid}/carrier-assignments")]
public class CarrierAssignmentController : ControllerBase
{
    private readonly ICarrierAssignmentService _service;
    private readonly IPermissionService _permissions;

    public CarrierAssignmentController(ICarrierAssignmentService service, IPermissionService permissions)
    {
        _service = service;
        _permissions = permissions;
    }

    [HttpGet]
    public async Task<IActionResult> GetByLoad(Guid loadId)
    {
        var assignments = await _service.GetByLoadAsync(loadId, User.GetUserId());
        return Ok(assignments);
    }

    [HttpPost("tender")]
    public async Task<IActionResult> Tender(
        Guid loadId,
        [FromBody] TenderCarrierDto dto)
    {
        if (!await HasAnyPermissionAsync(Permission.CarrierOffer_Create, Permission.Load_Tender))
            return Forbid();

        dto.LoadId = loadId;
        var id = await _service.TenderAsync(dto, User.GetUserId());
        return Ok(new { assignmentId = id });
    }

    [HttpPost("{assignmentId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid assignmentId)
    {
        if (!await HasAnyPermissionAsync(Permission.CarrierOffer_Accept, Permission.Load_Tender))
            return Forbid();

        await _service.AcceptAsync(assignmentId, User.GetUserId());
        return NoContent();
    }

    [HttpPost("{assignmentId:guid}/reject")]
    public async Task<IActionResult> Reject(Guid assignmentId)
    {
        if (!await HasAnyPermissionAsync(Permission.CarrierOffer_Reject, Permission.Load_Tender))
            return Forbid();

        await _service.RejectAsync(assignmentId, User.GetUserId());
        return NoContent();
    }

    private async Task<bool> HasAnyPermissionAsync(params Permission[] permissions)
    {
        var userId = User.GetUserId();
        foreach (var permission in permissions)
        {
            if (await _permissions.HasPermissionAsync(userId, permission))
                return true;
        }

        return false;
    }
}
