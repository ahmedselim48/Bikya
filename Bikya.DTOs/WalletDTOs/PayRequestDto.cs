namespace Bikya.DTOs.WalletDTOs
{
    public class PayRequestDto
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public string? Description { get; set; }
    }

}
