using Bikya.Data.Models;
using Bikya.Data.Response;
using Bikya.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bikya.Services.Interfaces;


namespace Bikya.Services.Services
{

    public class CategoryService :ICategoryService
    {
        private readonly BikyaContext _context;

        public CategoryService(BikyaContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .ToListAsync();

            var result = categories.Select(ToCategoryDTO).ToList();
            return ApiResponse<List<CategoryDTO>>.SuccessResponse(result, "Categories retrieved successfully");
        }

        public async Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return ApiResponse<CategoryDTO>.ErrorResponse("Category not found", 404);

            return ApiResponse<CategoryDTO>.SuccessResponse(ToCategoryDTO(category), "Category retrieved successfully");
        }

        public async Task<ApiResponse<CategoryDTO>> GetByNameAsync(string name)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

            if (category == null)
                return ApiResponse<CategoryDTO>.ErrorResponse("Category not found", 404);

            return ApiResponse<CategoryDTO>.SuccessResponse(ToCategoryDTO(category), "Category retrieved successfully");
        }

        public async Task<ApiResponse<CategoryDTO>> AddAsync(CreateCategoryDTO dto)
        {
            //  check if a category with the same name already exists
            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());

            if (existing != null)
            {
                return ApiResponse<CategoryDTO>.ErrorResponse("Category name already exists", 400);
            }

            var category = ToCategoryFromCreateDTO(dto);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return ApiResponse<CategoryDTO>.SuccessResponse(ToCategoryDTO(category), "Category created successfully", 201);
        }


        public async Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return ApiResponse<CategoryDTO>.ErrorResponse("Category not found", 404);

            //  Check for duplicate name in other categories
            var existsWithSameName = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == dto.Name.ToLower());

            if (existsWithSameName)
                return ApiResponse<CategoryDTO>.ErrorResponse("Category name already exists", 400);

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IconUrl = dto.IconUrl;

            await _context.SaveChangesAsync();

            return ApiResponse<CategoryDTO>.SuccessResponse(ToCategoryDTO(category), "Category updated successfully");
        }


        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return ApiResponse<bool>.ErrorResponse("Category not found", 404);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully");
        }

        public CategoryDTO ToCategoryDTO(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                IconUrl = category.IconUrl,
                ParentCategoryId = category.ParentCategoryId,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                
            };
        }

        public Category ToCategoryFromCreateDTO(CreateCategoryDTO dto)
        {
            return new Category
            {
                Name = dto.Name,
                IconUrl = dto.IconUrl,
                ParentCategoryId = dto.ParentCategoryId,
                Description = dto.Description,

            };
        }
    }
}
