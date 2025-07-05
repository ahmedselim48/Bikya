namespace Bikya.API.Areas.Wallet.DTOs
{
    public class RefundRequestDto
    {
        public int UserId { get; set; }

        public int TransactionId { get; set; }
        public string Reason { get; set; }
    }

}
