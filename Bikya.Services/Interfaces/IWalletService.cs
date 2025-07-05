using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikya.API.Areas.Wallet.DTOs;
using Bikya.Data.Models;
using Bikya.Data.Response;


namespace Bikya.Services.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResponse<WalletDto>> CreateWalletAsync(int userId);
        Task<ApiResponse<decimal>> GetBalanceAsync(int userId);

        Task<ApiResponse<WalletDto>> DepositAsync(int userId, decimal amount, string? description = null);
        Task<ApiResponse<WalletDto>> WithdrawAsync(int userId, decimal amount, string? description = null);
        Task<ApiResponse<WalletDto>> PayAsync(int userId, decimal amount, int orderId, string? description = null);
        Task<ApiResponse<WalletDto>> RefundAsync(int userId, int transactionId, string reason);

        Task<ApiResponse<List<TransactionDto>>> GetTransactionsAsync(int userId);
        Task<ApiResponse<TransactionDto>> GetTransactionByIdAsync(int userId, int id);

        Task<ApiResponse<bool>> LockWalletAsync(int userId);
        Task<ApiResponse<bool>> ConfirmTransactionAsync(int transactionId);
        Task<ApiResponse<bool>> LinkPaymentMethodAsync(int userId, string methodName);


    }
}
