using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class Product
    {
       
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsForExchange { get; set; }

        public string Condition { get; set; } // "New", "Used", etc.

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }


}

