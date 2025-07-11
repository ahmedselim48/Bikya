using System.ComponentModel.DataAnnotations;

namespace Bikya.API.Areas.ReviewsArea.DTOs
{
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
