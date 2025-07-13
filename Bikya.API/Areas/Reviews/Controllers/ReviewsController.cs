
using Bikya.Data.Response;
using Bikya.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bikya.API.Areas.ReviewsArea.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateReviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ReviewDTO>.ErrorResponse("Invalid data", 400));

            var response = await _service.AddAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch", 400));

            var response = await _service.UpdateAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }

}
