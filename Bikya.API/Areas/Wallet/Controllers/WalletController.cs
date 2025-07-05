using Microsoft.AspNetCore.Mvc;
using Bikya.Services.Interfaces;
using Bikya.API.Areas.Wallet.DTOs;

namespace Bikya.API.Areas.Wallet.Controllers
{
    [Area("Wallet")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        //  Create Wallet
        [HttpPost("create")]
        public async Task<ActionResult> CreateWallet([FromBody] UserIdRequestDto dto)
        {
            var result = await _walletService.CreateWalletAsync(dto.UserId);
            return Ok(result);
        }

        //  Get Balance
        [HttpGet("balance")]
        public async Task<ActionResult> GetBalance([FromQuery] int userId)
        {
            var result = await _walletService.GetBalanceAsync(userId);
            return Ok(result);
        }

        // Deposit
        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit([FromBody] DepositRequestDto dto)
        {
            var result = await _walletService.DepositAsync(dto.UserId, dto.Amount, dto.Description);
            return Ok(result);
        }

        //  Withdraw
        [HttpPost("withdraw")]
        public async Task<ActionResult> Withdraw([FromBody] WithdrawRequestDto dto)
        {
            var result = await _walletService.WithdrawAsync(dto.UserId, dto.Amount, dto.Description);
            return Ok(result);
        }

        //  Pay
        [HttpPost("pay")]
        public async Task<ActionResult> Pay([FromBody] PayRequestDto dto)
        {
            var result = await _walletService.PayAsync(dto.UserId, dto.Amount, dto.OrderId, dto.Description);
            return Ok(result);
        }

        //  Refund
        [HttpPost("refund")]
        public async Task<ActionResult> Refund([FromBody] RefundRequestDto dto)
        {
            var result = await _walletService.RefundAsync(dto.UserId, dto.TransactionId, dto.Reason);
            return Ok(result);
        }

        //  Get All Transactions
        [HttpGet("transactions")]
        public async Task<ActionResult> GetTransactions([FromQuery] int userId)
        {
            var result = await _walletService.GetTransactionsAsync(userId);
            return Ok(result);
        }

        //  Get Transaction by Id
        [HttpGet("transaction/{id}")]
        public async Task<ActionResult> GetTransactionById(int id, [FromQuery] int userId)
        {
            var result = await _walletService.GetTransactionByIdAsync(userId, id);
            return Ok(result);
        }

        //  Lock Wallet
        [HttpPost("lock")]
        public async Task<ActionResult> LockWallet([FromBody] UserIdRequestDto dto)
        {
            var result = await _walletService.LockWalletAsync(dto.UserId);
            return Ok(result);
        }

        //  Confirm Transaction
        [HttpPost("confirm/{transactionId}")]
        public async Task<ActionResult> ConfirmTransaction(int transactionId)
        {
            var result = await _walletService.ConfirmTransactionAsync(transactionId);
            return Ok(result);
        }

        //  Link Payment Method
        [HttpPost("link-method")]
        public async Task<ActionResult> LinkPaymentMethod([FromBody] LinkPaymentDto dto)
        {
            var result = await _walletService.LinkPaymentMethodAsync(dto.UserId, dto.MethodName);
            return Ok(result);
        }
    }
}
