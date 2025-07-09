using System;
using System.ComponentModel.DataAnnotations;

namespace Bikya.Data.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; } // Nullable string
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ReviewerId { get; set; }
        public required ApplicationUser Reviewer { get; set; } // Required navigation property

        public int SellerId { get; set; }
        public required ApplicationUser Seller { get; set; } // Required navigation property

        public int ProductId { get; set; }
        public required Product Product { get; set; } // Required navigation property

        public int OrderId { get; set; }
        public required Order Order { get; set; } // Required navigation property
    }
}