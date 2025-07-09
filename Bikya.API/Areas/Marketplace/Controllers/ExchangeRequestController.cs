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
    public class ExchangeRequestController : ControllerBase
    {
        private readonly IExchangeRequestService _exchangeRequestService;

        public ExchangeRequestController(IExchangeRequestService exchangeRequestService)
        {
            _exchangeRequestService = exchangeRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExchangeRequest([FromBody] Bikya.Services.Interfaces.CreateExchangeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data provided" });

            var result = await _exchangeRequestService.CreateExchangeRequestAsync(dto);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeRequest(int id)
        {
            var result = await _exchangeRequestService.GetExchangeRequestByIdAsync(id);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExchangeRequests()
        {
            var result = await _exchangeRequestService.GetAllExchangeRequestsAsync();
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExchangeRequest(int id, [FromBody] Bikya.Services.Interfaces.UpdateExchangeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data provided" });

            var result = await _exchangeRequestService.UpdateExchangeRequestAsync(id, dto);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchangeRequest(int id)
        {
            var result = await _exchangeRequestService.DeleteExchangeRequestAsync(id);
            return StatusCode(result.StatusCode, new { message = result.Message, data = result.Data });
        }
    }
}