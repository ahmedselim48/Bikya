using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsMain { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}