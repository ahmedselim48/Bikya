using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Models
{
    public class ExchangeRequest
    {
        public int Id { get; set; }
        public int OfferedProductId { get; set; }
        public Product OfferedProduct { get; set; }

        public int RequestedProductId { get; set; }
        public Product RequestedProduct { get; set; }

        public ExchangeStatus Status { get; set; } = ExchangeStatus.Pending;
        public DateTime RequestedAt { get; set; }

        public string? Message { get; set; }  // optional note from sender
    }

    public enum ExchangeStatus
    {
        Pending,
        Accepted,
        Rejected
    }

}
