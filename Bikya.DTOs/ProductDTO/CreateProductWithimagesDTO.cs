using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.DTOs.ProductDTO
{
    public class CreateProductWithimagesDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }

        public bool IsForExchange { get; set; }

        [Required]
        [StringLength(50)]
        public string Condition { get; set; }

        //public int CategoryId { get; set; }

        [Required]
        public IFormFile MainImage { get; set; }

        public List<IFormFile>? AdditionalImages { get; set; }

    }
}
