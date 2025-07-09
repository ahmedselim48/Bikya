using Bikya.Data.Response;
using Bikya.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bikya.API.Areas.Marketplace.Controllers
{
    [Area("Marketplace")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Bikya.Services.Interfaces.CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data provided" });

            var result = await _orderService.CreateOrderAsync(dto);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Bikya.Services.Interfaces.UpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data provided" });

            var result = await _orderService.UpdateOrderAsync(id, dto);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }
    }
}