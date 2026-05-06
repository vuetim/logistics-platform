using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderQueryService _queries;
        private readonly IOrderService _service; // do përdoret për POST/PUT/PATCH
        private readonly ILoadService _loadService;


        public OrdersController(
            IOrderQueryService queries,
            IOrderService service, ILoadService loadService)
        {
            _queries = queries;
            _service = service;
            _loadService = loadService;

        }

        // =============================
        // GET LIST
        // =============================
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] OrderQueryParameters parameters)
        {
            var result = await _queries.GetPagedAsync(parameters);
            return Ok(result);
        }

       

            // =============================
            // GET DETAILS
            // =============================
            [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result = await _queries.GetDetailsAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var orderId = await _service.CreateAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetDetails),
                new { id = orderId },
                null
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPost("{id:guid}/submit")]
        public async Task<IActionResult> Submit(Guid id)
        {
            await _service.ChangeStatusAsync(id, LogisticsPlatform.Domain.Enums.OrderStatus.Submitted);
            return NoContent();
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.ChangeStatusAsync(id, LogisticsPlatform.Domain.Enums.OrderStatus.Cancelled);
            return NoContent();
        }


        //[HttpPost("{orderId}/loads")]
        //public async Task<IActionResult> CreateLoadFromOrder(
        //   Guid orderId,
        //   [FromBody] CreateLoadFromOrderDto dto)
        //{
        //    // 🔐 Auth
        //    var userId = Guid.Parse(
        //        User.FindFirstValue(ClaimTypes.NameIdentifier)!
        //    );

        //    // ✅ Enforce path → body consistency
        //    dto.OrderId = orderId;

        //    // ⚙️ Delegate to LoadService
        //    var loadId = await _loadService.CreateFromOrderAsync(dto, userId);

        //    // 📍 REST response
        //    return CreatedAtRoute(
        //        routeName: "GetLoadDetails",
        //        routeValues: new { id = loadId },
        //        value: null
        //    );
        //}
    }
}
