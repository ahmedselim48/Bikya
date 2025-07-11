using Bikya.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Bikya.API.Areas.ReviewsArea.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ReviewerId { get; set; }
        public required ApplicationUser Reviewer { get; set; }

        public int SellerId { get; set; }
        public required ApplicationUser Seller { get; set; }


    }
}