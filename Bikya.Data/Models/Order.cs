using Bikya.Data.Enums;

namespace Bikya.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }

        public int SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal SellerAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public ShippingInfo ShippingInfo { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

   

}