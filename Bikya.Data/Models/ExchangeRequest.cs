using Bikya.Data.Enums;
using System;
using System.Collections.Generic;

namespace Bikya.Data.Models
{
    public class ExchangeRequest
    {
        public int Id { get; set; }
        public int OfferedProductId { get; set; }
        public required Product OfferedProduct { get; set; } // Required navigation property

        public int RequestedProductId { get; set; }
        public required Product RequestedProduct { get; set; } // Required navigation property

        public ExchangeStatus Status { get; set; } = ExchangeStatus.Pending;
        public DateTime RequestedAt { get; set; }

        public string? Message { get; set; } // Nullable string
    }
}