
using Bikya.API.Areas.ReviewsArea.DTOs;
using Bikya.Data;
using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class ReviewService
{
    private readonly BikyaContext _context;

    public ReviewService(BikyaContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews
            .Include(r => r.Reviewer)
            .Include(r => r.Seller)
            .Include(r => r.Product)
            .Include(r => r.Order)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Reviewer)
            .Include(r => r.Seller)
            .Include(r => r.Product)
            .Include(r => r.Order)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(UpdateReviewDTO dto)
    {
        var existing = await _context.Reviews.FindAsync(dto.Id);
        if (existing == null) return false;

        existing.Rating = dto.Rating;
        existing.Comment = dto.Comment;
        existing.ReviewerId = dto.ReviewerId;
        existing.SellerId = dto.SellerId;
        existing.ProductId = dto.ProductId;
        existing.OrderId = dto.OrderId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }

    public ReviewDTO ToDTO(Review review)
    {
        return new()
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            ReviewerId = review.ReviewerId,
            SellerId = review.SellerId,
            ProductId = review.ProductId,
            OrderId = review.OrderId
        };
    }

    public Review FromCreateDTO(CreateReviewDTO dto)
    {
        return new()
        {
            Rating = dto.Rating,
            Comment = dto.Comment,
            ReviewerId = dto.ReviewerId,
            SellerId = dto.SellerId,
            ProductId = dto.ProductId,
            OrderId = dto.OrderId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
