using Bikya.API.Areas.ReviewsArea.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bikya.API.Areas.ReviewsArea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _service;

        public ReviewsController(ReviewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _service.GetAllAsync();
            var result = reviews.Select(r => _service.ToDTO(r));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _service.GetByIdAsync(id);
            if (review == null) return NotFound();

            return Ok(_service.ToDTO(review));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateReviewDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var review = _service.FromCreateDTO(dto);
            await _service.AddAsync(review);

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, _service.ToDTO(review));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var updated = await _service.UpdateAsync(dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }


}
