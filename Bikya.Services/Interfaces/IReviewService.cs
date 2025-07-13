using Bikya.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ApiResponse<List<ReviewDTO>>> GetAllAsync();
        Task<ApiResponse<ReviewDTO>> GetByIdAsync(int id);
        Task<ApiResponse<ReviewDTO>> AddAsync(CreateReviewDTO dto);
        Task<ApiResponse<ReviewDTO>> UpdateAsync(int id, UpdateReviewDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
