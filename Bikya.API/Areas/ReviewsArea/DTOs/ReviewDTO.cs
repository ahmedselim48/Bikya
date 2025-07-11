namespace Bikya.API.Areas.ReviewsArea.DTOs
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
    }


}
