namespace Bikya.DTOs.WalletDTOs
{
    public class RefundRequestDto
    {
        public int UserId { get; set; }

        public int TransactionId { get; set; }
        public string Reason { get; set; }
    }

}
