using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public  class User
    {
        public int Id {  get; set; }
        public ICollection<Review> ReviewsWritten { get; set; }  // buyer
        public ICollection<Review> ReviewsReceived { get; set; } // seller
    }
}
