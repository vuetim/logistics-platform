using LogisticsPlatform.Application.DTOs.Financial;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Security;
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

        public CarrierSettlementController(
            ICarrierSettlementService service,
            IPdfService pdf)
        {
            _service = service;
            _pdf = pdf;
        }

        // =======================
        // GET settlement by loadId
        // Turvo-style: Get OR Create draft
        // =======================
        [HttpGet]
        public async Task<IActionResult> Get(Guid loadId)
        {
            // Nëse ekziston settlement → kthehet
            // Nëse nuk ekziston → krijohet auto draft nga load (GetAsync = GetOrCreate)
            var settlement = await _service.GetAsync(loadId);
            return Ok(settlement);
        }

        // =======================
        // CREATE settlement (manual)
        // (nëse don me e kriju explicit, krahas auto-create)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Create(Guid loadId, [FromBody] CreateSettlementDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var settlement = await _service.CreateAsync(loadId, dto, userId);
            return Ok(settlement);
        }

        // =======================
        // DOWNLOAD settlement PDF (manual generate)
        // Lejohet edhe pa delivered – gjeneron nga gjendja aktuale
        // =======================
        [HttpGet("{settlementId:guid}/pdf")]
        public async Task<IActionResult> GetPdf(Guid loadId, Guid settlementId)
        {
            var settlement = await _service.GetByIdAsync(settlementId);
            var bytes = _pdf.GenerateCarrierSettlementPdf(settlement);

            return File(bytes, "application/pdf",
                $"carrier-settlement-{settlementId}.pdf");
        }
    }
}
