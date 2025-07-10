using Bikya.Data;
using Bikya.Data.Enums;
using Bikya.Data.Models;
using Bikya.Data.Response;
using Bikya.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikya.DTOs.WalletDTOs;


namespace Bikya.Services.Services
{
    public class WalletService : IWalletService
    {
        #region BikyaContext
        private readonly BikyaContext context;

        public WalletService(BikyaContext context)
        {
            this.context = context;
        }
        #endregion


        #region ConfirmTransaction
        public async Task<ApiResponse<bool>> ConfirmTransactionAsync(int transactionId)
        {
            var transaction = await context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
                return ApiResponse<bool>.ErrorResponse("Transaction not found", 404);

            if (transaction.Status == TransactionStatus.Completed)
                return ApiResponse<bool>.ErrorResponse("Transaction already completed", 400);

            if (transaction.Status == TransactionStatus.Cancelled || transaction.Status == TransactionStatus.Failed)
                return ApiResponse<bool>.ErrorResponse("Cannot confirm a failed or cancelled transaction", 400);

            transaction.Status = TransactionStatus.Completed;

            await context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Transaction confirmed successfully");
        }
        #endregion


        #region CreateWallet
        async Task<ApiResponse<WalletDto>> IWalletService.CreateWalletAsync(int userId)
        {
            var existing = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (existing != null)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet already exists", 400);

            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            context.Wallets.Add(wallet);
            await context.SaveChangesAsync();

            // تحويل الـ Wallet إلى WalletDto يدويًا
            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                LinkedPaymentMethod = wallet.LinkedPaymentMethod,
                CreatedAt = wallet.CreatedAt
            };

            return ApiResponse<WalletDto>.SuccessResponse(walletDto, "Wallet created", 201);
        }

        #endregion

        #region Deposit

        async Task<ApiResponse<WalletDto>> IWalletService.DepositAsync(int userId, decimal amount, string? description)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet not found", 404);

            wallet.Balance += amount;

            context.Transactions.Add(new Transaction
            {
                Amount = amount,
                Type = TransactionType.Deposit,
                WalletId = wallet.Id,
                Description = description,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var dto = new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                UserId = wallet.UserId,
                LinkedPaymentMethod = wallet.LinkedPaymentMethod,
                CreatedAt = wallet.CreatedAt
            };

            return ApiResponse<WalletDto>.SuccessResponse(dto, "Deposit successful");
        }

        #endregion

        #region GetBalance
        async Task<ApiResponse<decimal>> IWalletService.GetBalanceAsync(int userId)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<decimal>.ErrorResponse("Wallet not found", 404);

            return ApiResponse<decimal>.SuccessResponse(wallet.Balance);
        }
        #endregion

        #region GetTransactionById
        async Task<ApiResponse<TransactionDto>> IWalletService.GetTransactionByIdAsync(int userId, int id)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<TransactionDto>.ErrorResponse("Wallet not found", 404);

            var transaction = await context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.WalletId == wallet.Id);

            if (transaction == null)
                return ApiResponse<TransactionDto>.ErrorResponse("Transaction not found", 404);

            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type.ToString(),
                Description = transaction.Description,
                Status = transaction.Status.ToString(),
                CreatedAt = transaction.CreatedAt
            };

            return ApiResponse<TransactionDto>.SuccessResponse(transactionDto);
        }

        #endregion

        #region GetTransactions
        async Task<ApiResponse<List<TransactionDto>>> IWalletService.GetTransactionsAsync(int userId)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<List<TransactionDto>>.ErrorResponse("Wallet not found", 404);

            var transactions = await context.Transactions
                .Where(t => t.WalletId == wallet.Id)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var transactionDtos = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type.ToString(),          
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                Status = t.Status.ToString()        
            }).ToList();


            return ApiResponse<List<TransactionDto>>.SuccessResponse(transactionDtos);
        }
        #endregion


        #region LinkPaymentMethod
        async Task<ApiResponse<bool>> IWalletService.LinkPaymentMethodAsync(int userId, string methodName)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<bool>.ErrorResponse("Wallet not found", 404);

            wallet.LinkedPaymentMethod = methodName;
            await context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, $"Payment method '{methodName}' linked successfully");
        } 
        #endregion

        #region LockWallet
        async Task<ApiResponse<bool>> IWalletService.LockWalletAsync(int userId)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<bool>.ErrorResponse("Wallet not found", 404);

            if (wallet.IsLocked)
                return ApiResponse<bool>.ErrorResponse("Wallet is already locked", 400);

            wallet.IsLocked = true;
            await context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Wallet locked successfully");

        }
        #endregion

        #region Payment
        async Task<ApiResponse<WalletDto>> IWalletService.PayAsync(int userId, decimal amount, int orderId, string? description)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet not found", 404);

            if (wallet.IsLocked)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet is locked", 403);

            if (wallet.Balance < amount)
                return ApiResponse<WalletDto>.ErrorResponse("Insufficient balance", 400);

            wallet.Balance -= amount;

            context.Transactions.Add(new Transaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = TransactionType.Payment,
                Description = $"Payment for Order #{orderId} - {description}",
                Status = TransactionStatus.Completed,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                LinkedPaymentMethod = wallet.LinkedPaymentMethod
            };

            return ApiResponse<WalletDto>.SuccessResponse(walletDto, "Payment successful");
        }
        #endregion


        #region Refund
        async Task<ApiResponse<WalletDto>> IWalletService.RefundAsync(int userId, int transactionId, string reason)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet not found", 404);

            var originalTransaction = await context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.WalletId == wallet.Id);

            if (originalTransaction == null)
                return ApiResponse<WalletDto>.ErrorResponse("Original transaction not found", 404);

            if (originalTransaction.Type != TransactionType.Payment)
                return ApiResponse<WalletDto>.ErrorResponse("Only payments can be refunded", 400);

            if (originalTransaction.Status != TransactionStatus.Completed)
                return ApiResponse<WalletDto>.ErrorResponse("Transaction is not completed", 400);

            wallet.Balance += originalTransaction.Amount;

            context.Transactions.Add(new Transaction
            {
                WalletId = wallet.Id,
                Amount = originalTransaction.Amount,
                Type = TransactionType.Refund,
                Description = $"Refund for Transaction #{transactionId} - {reason}",
                Status = TransactionStatus.Completed,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                LinkedPaymentMethod = wallet.LinkedPaymentMethod
            };

            return ApiResponse<WalletDto>.SuccessResponse(walletDto, "Refund completed successfully");
        }
        #endregion


        #region Withdraw

        async Task<ApiResponse<WalletDto>> IWalletService.WithdrawAsync(int userId, decimal amount, string? description)
        {
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return ApiResponse<WalletDto>.ErrorResponse("Wallet not found", 404);

            if (wallet.Balance < amount)
                return ApiResponse<WalletDto>.ErrorResponse("Insufficient balance", 400);

            wallet.Balance -= amount;

            context.Transactions.Add(new Transaction
            {
                Amount = amount,
                Type = TransactionType.Withdraw,
                WalletId = wallet.Id,
                Description = description,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var walletDto = new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                LinkedPaymentMethod = wallet.LinkedPaymentMethod
            };

            return ApiResponse<WalletDto>.SuccessResponse(walletDto, "Withdraw successful");
        }

        #endregion

    }
}
