namespace Bikya.DTOs.WalletDTOs
{
    public class DepositRequestDto
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}
