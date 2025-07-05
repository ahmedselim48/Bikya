using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Path { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200, string? path = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Path = path
            };
        }
        public static ApiResponse<T> ErrorResponse(string message = "Something went wrong", int statusCode = 500, List<string>? errors = null, string? path = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors,
                Path = path
            };
        }
    }
}
