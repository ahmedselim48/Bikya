using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public  class User
    {
        public int Id {  get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string? ProfileImageUrl { get; set; }

        public bool IsVerified { get; set; }

        public ICollection<Product>? Products { get; set; }

        public Wallet? Wallet { get; set; }
        public ICollection<Review>? ReviewsWritten { get; set; }  // buyer
        public ICollection<Review>? ReviewsReceived { get; set; } // seller
    }
}

