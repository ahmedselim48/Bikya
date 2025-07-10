using Bikya.Data.Enums;
using global::Bikya.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bikya.API.Areas.Marketplace.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int BuyerId { get; set; }
        [Required]
        public int SellerId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Platform fee cannot be negative")]
        public decimal PlatformFee { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Seller amount cannot be negative")]
        public decimal SellerAmount { get; set; }
        [Required]
        public ShippingInfoDto? ShippingInfo { get; set; }
    }

    public class UpdateOrderDto
    {
        [Required]
        public OrderStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Platform fee cannot be negative")]
        public decimal PlatformFee { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Seller amount cannot be negative")]
        public decimal SellerAmount { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required string ProductName { get; set; } // Required string
        public int BuyerId { get; set; }
        public required string BuyerName { get; set; } // Required string
        public int SellerId { get; set; }
        public required string SellerName { get; set; } // Required string
        public decimal TotalAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal SellerAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public required ShippingInfoDto ShippingInfo { get; set; } // Required navigation property
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>(); // Initialized to empty list
    }

    public class ShippingInfoDto
    {
        [Required]
        public required string RecipientName { get; set; } // Required string
        [Required]
        public required string Address { get; set; } // Required string
        [Required]
        public required string City { get; set; } // Required string
        [Required]
        public required string PostalCode { get; set; } // Required string
        [Required]
        public required string PhoneNumber { get; set; } // Required string
    }

    public class ReviewDto
    {
        public int Id { get; set; }
        public string? Comment { get; set; } // Nullable string
        public int Rating { get; set; }
    }
}