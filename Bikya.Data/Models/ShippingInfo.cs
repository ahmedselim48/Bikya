using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class ShippingInfo
    {
        public int ShippingId {  get; set; }

        public string RecipientName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public ShippingStatus status { get; set; }

        public DateTime CreateAt { get; set; } 

        public int OrderId { get; set; }
        public Order Order { get; set; }



    }
}
