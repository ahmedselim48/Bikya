using System;
using Bikya.Data.Enums;

namespace Bikya.Data.Models
{
    public class ShippingInfo
    {
        public int ShippingId { get; set; }
        public required string RecipientName { get; set; } // Required string
        public required string Address { get; set; } // Required string
        public required string City { get; set; } // Required string
        public required string PostalCode { get; set; } // Required string
        public required string PhoneNumber { get; set; } // Required string
        public ShippingStatus Status { get; set; }
        public DateTime CreateAt { get; set; }
        public int OrderId { get; set; }
        public required Order Order { get; set; } // Required navigation property
    }
}