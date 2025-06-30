using Bikya.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public TransactionType Type { get; set; }

        public string? Description { get; set; }

        // Foreign key to Wallet
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }

}
