using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.DTOs.ProductDTO
{
    public class CreateImageDTO
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public bool IsMain { get; set; }
    }
}
