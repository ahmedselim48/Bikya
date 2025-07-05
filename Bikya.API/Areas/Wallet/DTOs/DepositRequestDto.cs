namespace Bikya.API.Areas.Wallet.DTOs
{
    public class DepositRequestDto
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
