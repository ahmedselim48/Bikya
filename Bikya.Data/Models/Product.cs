using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bikya.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public required string Title { get; set; } // Required string
        public required string Description { get; set; } // Required string
        public decimal Price { get; set; }
        public bool IsForExchange { get; set; }
        public required string Condition { get; set; } // Required string
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public required ApplicationUser User { get; set; } // Required navigation property

        public List<Review>? Reviews { get; set; } // Nullable collection
        public List<ProductImage>? Images { get; set; } // Nullable collection

        public int CategoryId { get; set; }
        public required Category Category { get; set; } // Required navigation property
    }
}