namespace Bikya.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}