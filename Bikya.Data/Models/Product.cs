using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
