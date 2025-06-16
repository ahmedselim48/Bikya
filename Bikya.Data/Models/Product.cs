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

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsForExchange { get; set; }

        [Required]
        public string Condition { get; set; } // "New", "Used", etc.

       
        public DateTime CreatedAt { get; set; }

        

        public int UserId { get; set; }

        public User User { get; set; }


        public ICollection<Review> Reviews { get; set; }

        public ICollection<ProductImage> Images { get; set; }
       
    }

  
}
// add defult to CreatedAt
