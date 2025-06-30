using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public  class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Address { get; set; }
        public bool IsVerified { get; set; }

        public ICollection<Product>? Products { get; set; }
        public Wallet? Wallet { get; set; }
        public ICollection<Review>? ReviewsWritten { get; set; }
        public ICollection<Review>? ReviewsReceived { get; set; }
        public ICollection<Order>? OrdersBought { get; set; }
        public ICollection<Order>? OrdersSold { get; set; }

    }
}

