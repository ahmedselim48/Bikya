using Bikya.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bikya.Services.Interfaces
{
    public interface ICategoryService
    {
        
        Task<ApiResponse<List<CategoryDTO>>> GetAllAsync();
        Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id);
        Task<ApiResponse<CategoryDTO>> GetByNameAsync(string name);
        Task<ApiResponse<CategoryDTO>> AddAsync(CreateCategoryDTO dto);
        Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }

}
