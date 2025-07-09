using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Bikya.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public required string FullName { get; set; } // Required string
        public string? ProfileImageUrl { get; set; } // Nullable string
        public string? Address { get; set; } // Nullable string
        public bool IsVerified { get; set; }
        public List<Product>? Products { get; set; } // Nullable collection
        public Wallet? Wallet { get; set; } // Nullable navigation property
        public List<Review>? ReviewsWritten { get; set; } // Nullable collection
        public List<Review>? ReviewsReceived { get; set; } // Nullable collection
        public List<Order>? OrdersBought { get; set; } // Nullable collection
        public List<Order>? OrdersSold { get; set; } // Nullable collection
    }
}