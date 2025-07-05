namespace Bikya.API.Areas.Wallet.DTOs
{
    public class LinkPaymentDto
    {
        public int UserId { get; set; }
        public string MethodName { get; set; } = string.Empty;
    }

}
