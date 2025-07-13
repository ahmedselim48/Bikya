using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Services.Interfaces
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReviewerId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
         public string SellerName { get; set; } = string.Empty;
         public string BuyerName { get; set; } = string.Empty;
    }
    public class CreateReviewDTO
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int ReviewerId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }
    }

    public class UpdateReviewDTO
    {
        [Required]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int ReviewerId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
