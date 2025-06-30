using System;
using System.ComponentModel.DataAnnotations;

namespace Bikya.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // علاقات
        public int ReviewerId { get; set; }
        public ApplicationUser Reviewer { get; set; }

        public int SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
