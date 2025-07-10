using Bikya.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bikya.Services.Interfaces
{
    public class CreateExchangeRequestDto
    {
        [Required]
        public int OfferedProductId { get; set; }
        [Required]
        public int RequestedProductId { get; set; }
        public string? Message { get; set; } // Nullable string
    }

    public class UpdateExchangeRequestDto
    {
        [Required]
        public ExchangeStatus Status { get; set; }
        public string? Message { get; set; } // Nullable string
    }

    public class ExchangeRequestResponseDto
    {
        public int Id { get; set; }
        public int OfferedProductId { get; set; }
        public required string OfferedProductName { get; set; } // Required string
        public int RequestedProductId { get; set; }
        public required string RequestedProductName { get; set; } // Required string
        public ExchangeStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public string? Message { get; set; } // Nullable string
    }
}