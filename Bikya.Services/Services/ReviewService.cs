using Bikya.Data.Models;
using Bikya.Data.Response;
using Bikya.Data;
using Bikya.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bikya.Data.Enums;

namespace Bikya.Services.Services
{
    public class ReviewService : IReviewService
    {
        private readonly BikyaContext _context;

        public ReviewService(BikyaContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ReviewDTO>>> GetAllAsync()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Seller)
                .Include(r => r.Order)
                .ToListAsync();

            var result = reviews.Select(ToReviewDTO).ToList();
            return ApiResponse<List<ReviewDTO>>.SuccessResponse(result, "Reviews retrieved successfully");
        }

        public async Task<ApiResponse<ReviewDTO>> GetByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Seller)
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("Review not found", 404);

            return ApiResponse<ReviewDTO>.SuccessResponse(ToReviewDTO(review), "Review retrieved successfully");
        }

        public async Task<ApiResponse<ReviewDTO>> AddAsync(CreateReviewDTO dto)
        {
            var reviewer = await _context.Users.FindAsync(dto.ReviewerId);
            if (reviewer == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("Reviewer not found", 404);

            var seller = await _context.Users.FindAsync(dto.SellerId);
            if (seller == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("Seller not found", 404);

            var order = await _context.Orders.FirstOrDefaultAsync(o =>
                 o.Id == dto.OrderId &&
                 o.BuyerId == dto.ReviewerId &&
                 o.SellerId == dto.SellerId &&
                 o.Status == OrderStatus.Completed
             );

            if (order == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("You can only review sellers you've bought from.", 403);

            var existingReview = await _context.Reviews
              .AnyAsync(r => r.OrderId == dto.OrderId && r.ReviewerId == dto.ReviewerId);

            if (existingReview)
                return ApiResponse<ReviewDTO>.ErrorResponse("You have already reviewed this order.", 409);

            var review = new Review
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewerId = dto.ReviewerId,
                Reviewer = reviewer,
                SellerId = dto.SellerId,
                Seller = seller,
                OrderId = dto.OrderId,
                Order = order,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return ApiResponse<ReviewDTO>.SuccessResponse(ToReviewDTO(review), "Review created successfully", 201);
        }

        public async Task<ApiResponse<List<ReviewDTO>>> GetReviewsForSellerAsync(int sellerId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.SellerId == sellerId)
                .Select(r => new ReviewDTO
                {
                    ReviewerId = r.ReviewerId,
                    SellerId = r.SellerId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    OrderId = r.OrderId,

                })
                .ToListAsync();

            return ApiResponse<List<ReviewDTO>>.SuccessResponse(reviews);
        }


        public async Task<ApiResponse<ReviewDTO>> UpdateAsync(int id, UpdateReviewDTO dto)
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Seller)
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return ApiResponse<ReviewDTO>.ErrorResponse("Review not found", 404);

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            await _context.SaveChangesAsync();

            return ApiResponse<ReviewDTO>.SuccessResponse(ToReviewDTO(review), "Review updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return ApiResponse<bool>.ErrorResponse("Review not found", 404);

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Review deleted successfully");
        }

        public ReviewDTO ToReviewDTO(Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                ReviewerId = review.ReviewerId,
                SellerId = review.SellerId,
                OrderId = review.OrderId,
                BuyerName = review.Reviewer?.UserName,
                SellerName = review.Seller?.UserName,
            };
        }
    }
}
