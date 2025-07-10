
using Bikya.Data.Response;
using System.Threading.Tasks;

namespace Bikya.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int id);
        Task<ApiResponse<List<OrderResponseDto>>> GetAllOrdersAsync();
        Task<ApiResponse<OrderResponseDto>> UpdateOrderAsync(int id, UpdateOrderDto dto);
        Task<ApiResponse<bool>> DeleteOrderAsync(int id);
    }
}