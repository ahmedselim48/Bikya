using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bikya.DTOs.ProductDTO
{
    public class ProductImageDTO
    {
       
        public IFormFile Image { get; set; }
        public int ProductId { get; set; }

        public bool IsMain { get; set; }

    }
}
