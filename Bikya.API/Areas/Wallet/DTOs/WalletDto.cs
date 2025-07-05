namespace Bikya.API.Areas.Wallet.DTOs
{
    public class WalletDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public string? LinkedPaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
