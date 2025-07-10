using Bikya.Data.Response;
using System.Threading.Tasks;

namespace Bikya.Services.Interfaces
{
    public interface IExchangeRequestService
    {
        Task<ApiResponse<ExchangeRequestResponseDto>> CreateExchangeRequestAsync(CreateExchangeRequestDto dto);
        Task<ApiResponse<ExchangeRequestResponseDto>> GetExchangeRequestByIdAsync(int id);
        Task<ApiResponse<List<ExchangeRequestResponseDto>>> GetAllExchangeRequestsAsync();
        Task<ApiResponse<ExchangeRequestResponseDto>> UpdateExchangeRequestAsync(int id, UpdateExchangeRequestDto dto);
        Task<ApiResponse<bool>> DeleteExchangeRequestAsync(int id);
    }
}