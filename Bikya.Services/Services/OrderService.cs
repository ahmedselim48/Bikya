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
    public class OrderService : IOrderService
    {
        private readonly BikyaContext context;

        public OrderService(BikyaContext context)
        {
            this.context = context;
        }

        //public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(CreateOrderDto dto)
        //{
        //    try
        //    {
        //        var buyer = await context.Users.FindAsync(dto.BuyerId);
        //        var seller = await context.Users.FindAsync(dto.SellerId);
        //        var product = await context.Products.FindAsync(dto.ProductId);

        //        if (buyer == null || seller == null || product == null)
        //            return ApiResponse<OrderResponseDto>.ErrorResponse("Invalid Buyer, Seller, or Product", 400);

        //        if (dto.BuyerId == dto.SellerId)
        //            return ApiResponse<OrderResponseDto>.ErrorResponse("Buyer and Seller cannot be the same", 400);

        //        var order = new Order
        //        {
        //            ProductId = dto.ProductId,
        //            BuyerId = dto.BuyerId,
        //            SellerId = dto.SellerId,
        //            TotalAmount = dto.TotalAmount,
        //            PlatformFee = dto.PlatformFee,
        //            SellerAmount = dto.SellerAmount,
        //            CreatedAt = DateTime.UtcNow
        //        };
        //        order.ShippingInfo = new ShippingInfo
        //        {
        //            RecipientName = dto.ShippingInfo.RecipientName,
        //            Address = dto.ShippingInfo.Address,
        //            City = dto.ShippingInfo.City,
        //            PostalCode = dto.ShippingInfo.PostalCode,
        //            PhoneNumber = dto.ShippingInfo.PhoneNumber,
        //            CreateAt = DateTime.UtcNow,
        //            Order = order
        //        };


        //        context.Orders.Add(order);
        //        await context.SaveChangesAsync();

        //        // Reload order with related data
        //        var reloadedOrder = await context.Orders
        //            .Include(o => o.Product)
        //            .Include(o => o.Buyer)
        //            .Include(o => o.Seller)
        //            .Include(o => o.ShippingInfo)
        //            .Include(o => o.Reviews)
        //            .FirstOrDefaultAsync(o => o.Id == order.Id);

        //        var responseDto = MapToResponseDto(reloadedOrder);
        //        return ApiResponse<OrderResponseDto>.SuccessResponse(responseDto, "Order created successfully", 201);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<OrderResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
        //    }
        //}
        public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            try
            {
                var buyer = await context.Users.FindAsync(dto.BuyerId);
                var seller = await context.Users.FindAsync(dto.SellerId);
                var product = await context.Products.FindAsync(dto.ProductId);

                if (buyer == null || seller == null || product == null)
                    return ApiResponse<OrderResponseDto>.ErrorResponse("Invalid Buyer, Seller, or Product", 400);

                if (dto.BuyerId == dto.SellerId)
                    return ApiResponse<OrderResponseDto>.ErrorResponse("Buyer and Seller cannot be the same", 400);

                if (dto.ShippingInfo == null)
                {
                    return ApiResponse<OrderResponseDto>.ErrorResponse("Shipping information is required", 400);
                }

                var order = new Order
                {
                    Product = product,
                    Buyer = buyer,
                    Seller = seller,
                    TotalAmount = dto.TotalAmount,
                    PlatformFee = dto.PlatformFee,
                    SellerAmount = dto.SellerAmount,
                    CreatedAt = DateTime.UtcNow,
                    ShippingInfo = new ShippingInfo
                    {
                        RecipientName = dto.ShippingInfo.RecipientName,
                        Address = dto.ShippingInfo.Address,
                        City = dto.ShippingInfo.City,
                        PostalCode = dto.ShippingInfo.PostalCode,
                        PhoneNumber = dto.ShippingInfo.PhoneNumber,
                        CreateAt = DateTime.UtcNow,
                        Order = null // Temporary assignment to avoid CS9035  
                    }
                };

                // Fix CS9035 by explicitly setting the required member after initialization  
                order.ShippingInfo.Order = order;

                context.Orders.Add(order);
                await context.SaveChangesAsync();

                var reloadedOrder = await context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Buyer)
                    .Include(o => o.Seller)
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Reviews)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                var responseDto = MapToResponseDto(reloadedOrder);
                return ApiResponse<OrderResponseDto>.SuccessResponse(responseDto, "Order created successfully", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }
        public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Buyer)
                    .Include(o => o.Seller)
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Reviews)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return ApiResponse<OrderResponseDto>.ErrorResponse("Order not found", 404);

                var responseDto = MapToResponseDto(order);
                return ApiResponse<OrderResponseDto>.SuccessResponse(responseDto, "Order retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<OrderResponseDto>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Buyer)
                    .Include(o => o.Seller)
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Reviews)
                    .ToListAsync();

                var responseDtos = orders.Select(MapToResponseDto).ToList();
                return ApiResponse<List<OrderResponseDto>>.SuccessResponse(responseDtos, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<OrderResponseDto>>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<OrderResponseDto>> UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            try
            {
                var order = await context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Buyer)
                    .Include(o => o.Seller)
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Reviews)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return ApiResponse<OrderResponseDto>.ErrorResponse("Order not found", 404);

                order.Status = dto.Status;
                order.PaidAt = dto.PaidAt;
                order.CompletedAt = dto.CompletedAt;
                order.TotalAmount = dto.TotalAmount;
                order.PlatformFee = dto.PlatformFee;
                order.SellerAmount = dto.SellerAmount;

                await context.SaveChangesAsync();

                var responseDto = MapToResponseDto(order);
                return ApiResponse<OrderResponseDto>.SuccessResponse(responseDto, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<OrderResponseDto>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                if (order == null)
                    return ApiResponse<bool>.ErrorResponse("Order not found", 404);

                context.Orders.Remove(order);
                await context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse($"A server error occurred: {ex.Message}", 500);
            }
        }

        private OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order?.Id ?? 0,
                ProductId = order?.ProductId ?? 0,
                ProductName = order?.Product?.Title ?? "Unknown Product",
                BuyerId = order?.BuyerId ?? 0,
                BuyerName = order?.Buyer?.FullName ?? "Unknown Buyer",
                SellerId = order?.SellerId ?? 0,
                SellerName = order?.Seller?.FullName ?? "Unknown Seller",
                TotalAmount = order?.TotalAmount ?? 0,
                PlatformFee = order?.PlatformFee ?? 0,
                SellerAmount = order?.SellerAmount ?? 0,
                Status = order?.Status ?? OrderStatus.Pending,
                CreatedAt = order?.CreatedAt ?? DateTime.UtcNow,
                PaidAt = order?.PaidAt,
                CompletedAt = order?.CompletedAt,
                ShippingInfo = order?.ShippingInfo != null ? new ShippingInfoDto
                {
                    RecipientName = order.ShippingInfo.RecipientName ?? "Unknown Recipient",
                    Address = order.ShippingInfo.Address ?? "Unknown Address",
                    City = order.ShippingInfo.City ?? "Unknown City",
                    PostalCode = order.ShippingInfo.PostalCode ?? "Unknown PostalCode",
                    PhoneNumber = order.ShippingInfo.PhoneNumber ?? "Unknown Phone"
                } : new ShippingInfoDto
                {
                    RecipientName = "Unknown Recipient",
                    Address = "Unknown Address",
                    City = "Unknown City",
                    PostalCode = "Unknown PostalCode",
                    PhoneNumber = "Unknown Phone"
                },
                Reviews = order?.Reviews?.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Comment = r.Comment ?? "",
                    Rating = r.Rating
                }).ToList() ?? new List<ReviewDto>()
            };
        }
    }
}