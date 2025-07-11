using Bikya.API.Areas.Category.Models;
using Bikya.API.Areas.CategoryArea.DTO;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        var result = categories.Select(c => _service.ToCategoryDTO(c));
        return Ok(result);
    }

    // GET: api/categories/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(_service.ToCategoryDTO(category));
    }

    // GET: api/categories/name/electronics
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var category = await _service.GetByNameAsync(name);
        if (category == null)
            return NotFound();

        return Ok(_service.ToCategoryDTO(category));
    }

    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = _service.ToCategoryFromCreateDTO(dto);
        await _service.AddAsync(category);

        var result = _service.ToCategoryDTO(category);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await _service.UpdateAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }


    // DELETE: api/categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

