using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers.Financial
{
    [ApiController]
    [Route("api/loads/{loadId:guid}/financials/settlements")]
    [Authorize]
    public class CarrierSettlementController : ControllerBase
    {
        private readonly ICarrierSettlementService _service;
        private readonly IPdfService _pdf;
        private readonly IPermissionService _permissions;

        public CarrierSettlementController(
            ICarrierSettlementService service,
            IPdfService pdf,
            IPermissionService permissions)
        {
            _service = service;
            _pdf = pdf;
            _permissions = permissions;
        }

        // GET settlement by loadId
        //  Get OR Create draft
        [HttpGet]
        public async Task<IActionResult> Get(Guid loadId)
        {
            if (!await _permissions.HasPermissionAsync(GetUserId(), Permission.Financial_View))
                return Forbid();

            var settlement = await _service.GetAsync(loadId);
            return Ok(settlement);
        }

        // CREATE settlement (manual)
        // (nëse don me e kriju explicit, krahas auto-create)
        [HttpPost]
        public async Task<IActionResult> Create(Guid loadId, [FromBody] CreateSettlementDto dto)
        {
            var userId = GetUserId();
            if (!await _permissions.HasPermissionAsync(userId, Permission.Financial_Settlement_UpdateStatus))
                return Forbid();

            var settlement = await _service.CreateAsync(loadId, dto, userId);
            return Ok(settlement);
        }

        // DOWNLOAD settlement PDF (manual generate)
        // Lejohet edhe pa delivered – gjeneron nga gjendja aktuale
        [HttpGet("{settlementId:guid}/pdf")]
        public async Task<IActionResult> GetPdf(Guid loadId, Guid settlementId)
        {
            if (!await _permissions.HasPermissionAsync(GetUserId(), Permission.Financial_View))
                return Forbid();

            var settlement = await _service.GetByIdAsync(settlementId);
            var bytes = _pdf.GenerateCarrierSettlementPdf(settlement);

            return File(bytes, "application/pdf",
                $"carrier-settlement-{settlementId}.pdf");
        }

        private Guid GetUserId()
            => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
