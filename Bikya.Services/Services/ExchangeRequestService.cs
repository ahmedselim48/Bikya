using Bikya.Data;
using Bikya.Data.Enums;
using Bikya.Data.Models;
using Bikya.Data.Response;
using Bikya.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikya.Services.Services
{
    public class ExchangeRequestService : IExchangeRequestService
    {
        private readonly BikyaContext context;

        public ExchangeRequestService(BikyaContext context)
        {
            this.context = context;
        }

        public async Task<ApiResponse<ExchangeRequestResponseDto>> CreateExchangeRequestAsync(CreateExchangeRequestDto dto)
        {
            try
            {
                var offeredProduct = await context.Products.FindAsync(dto.OfferedProductId);
                var requestedProduct = await context.Products.FindAsync(dto.RequestedProductId);

                if (offeredProduct == null || requestedProduct == null)
                    return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse("Invalid product IDs", 400);

                if (dto.OfferedProductId == dto.RequestedProductId)
                    return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse("Offered and requested products cannot be the same", 400);

                var exchangeRequest = new ExchangeRequest
                {
                    OfferedProductId = dto.OfferedProductId,
                    RequestedProductId = dto.RequestedProductId,
                    Message = dto.Message,
                    RequestedAt = DateTime.UtcNow
                };

                context.ExchangeRequests.Add(exchangeRequest);
                await context.SaveChangesAsync();

                // Reload exchange request with related data
                var reloadedRequest = await context.ExchangeRequests
                    .Include(er => er.OfferedProduct)
                    .Include(er => er.RequestedProduct)
                    .FirstOrDefaultAsync(er => er.Id == exchangeRequest.Id);

                var responseDto = MapToResponseDto(reloadedRequest);
                return ApiResponse<ExchangeRequestResponseDto>.SuccessResponse(responseDto, "Exchange request created successfully", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<ExchangeRequestResponseDto>> GetExchangeRequestByIdAsync(int id)
        {
            try
            {
                var exchangeRequest = await context.ExchangeRequests
                    .Include(er => er.OfferedProduct)
                    .Include(er => er.RequestedProduct)
                    .FirstOrDefaultAsync(er => er.Id == id);

                if (exchangeRequest == null)
                    return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse("Exchange request not found", 404);

                var responseDto = MapToResponseDto(exchangeRequest);
                return ApiResponse<ExchangeRequestResponseDto>.SuccessResponse(responseDto, "Exchange request retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<ExchangeRequestResponseDto>>> GetAllExchangeRequestsAsync()
        {
            try
            {
                var exchangeRequests = await context.ExchangeRequests
                    .Include(er => er.OfferedProduct)
                    .Include(er => er.RequestedProduct)
                    .ToListAsync();

                var responseDtos = exchangeRequests.Select(MapToResponseDto).ToList();
                return ApiResponse<List<ExchangeRequestResponseDto>>.SuccessResponse(responseDtos, "Exchange requests retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ExchangeRequestResponseDto>>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<ExchangeRequestResponseDto>> UpdateExchangeRequestAsync(int id, UpdateExchangeRequestDto dto)
        {
            try
            {
                var exchangeRequest = await context.ExchangeRequests
                    .Include(er => er.OfferedProduct)
                    .Include(er => er.RequestedProduct)
                    .FirstOrDefaultAsync(er => er.Id == id);

                if (exchangeRequest == null)
                    return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse("Exchange request not found", 404);

                exchangeRequest.Status = dto.Status;
                exchangeRequest.Message = dto.Message;

                await context.SaveChangesAsync();

                var responseDto = MapToResponseDto(exchangeRequest);
                return ApiResponse<ExchangeRequestResponseDto>.SuccessResponse(responseDto, "Exchange request updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ExchangeRequestResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteExchangeRequestAsync(int id)
        {
            try
            {
                var exchangeRequest = await context.ExchangeRequests.FindAsync(id);
                if (exchangeRequest == null)
                    return ApiResponse<bool>.ErrorResponse("Exchange request not found", 404);

                context.ExchangeRequests.Remove(exchangeRequest);
                await context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Exchange request deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        private ExchangeRequestResponseDto MapToResponseDto(ExchangeRequest exchangeRequest)
        {
            return new ExchangeRequestResponseDto
            {
                Id = exchangeRequest?.Id ?? 0,
                OfferedProductId = exchangeRequest?.OfferedProductId ?? 0,
                OfferedProductName = exchangeRequest?.OfferedProduct?.Title ?? "Unknown Product",
                RequestedProductId = exchangeRequest?.RequestedProductId ?? 0,
                RequestedProductName = exchangeRequest?.RequestedProduct?.Title ?? "Unknown Product",
                Status = exchangeRequest?.Status ?? ExchangeStatus.Pending,
                RequestedAt = exchangeRequest?.RequestedAt ?? DateTime.UtcNow,
                Message = exchangeRequest?.Message ?? ""
            };
        }
    }
}