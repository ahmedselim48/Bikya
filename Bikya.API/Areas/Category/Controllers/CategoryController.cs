using Microsoft.EntityFrameworkCore;
using Bikya.Data.Response;
using Bikya.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Bikya.Services.Interfaces;




[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
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

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var response = await _service.GetByNameAsync(name);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> Add([FromBody] CreateCategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<CategoryDTO>.ErrorResponse("Invalid data", 400));

        var response = await _service.AddAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
    {
        if (id != dto.Id)
            return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch", 400));

        var response = await _service.UpdateAsync(id, dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return StatusCode(response.StatusCode, response);
    }
}