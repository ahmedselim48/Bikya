using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        public decimal Balance { get; set; } = 0;

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LinkedPaymentMethod { get; set; }
        public bool IsLocked { get; set; } = false;


    }
}
