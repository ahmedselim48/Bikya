using Bikya.API.Areas.Category.Models;
using Bikya.API.Areas.CategoryArea.DTO;
using Bikya.Data;
using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;


public class CategoryService
{
    private readonly BikyaContext _context;

    public CategoryService(BikyaContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDTO dto)
    {
        var existing = await _context.Categories.FindAsync(dto.Id);
        if (existing == null) return false;

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.IconUrl = dto.IconUrl;
        existing.ParentCategoryId = dto.ParentCategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public ShowCategoryDTO ToDTO(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        IconUrl = c.IconUrl,
        ParentCategoryId = c.ParentCategoryId,
        CreatedAt = c.CreatedAt
    };

    public Category FromCreateDTO(CreateCategoryDTO dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        IconUrl = dto.IconUrl,
        ParentCategoryId = dto.ParentCategoryId,
        CreatedAt = DateTime.UtcNow
    };
}






