using Bikya.Data.Enums;
using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Configurations
{
    public class ExchangeRequestConfiguration : IEntityTypeConfiguration<ExchangeRequest>
    {
        public void Configure(EntityTypeBuilder<ExchangeRequest> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Status)
                   .HasConversion<string>() // يخزن كـ نص (أفضل للقراءة)
                   .HasDefaultValue(ExchangeStatus.Pending);

            builder.Property(e => e.RequestedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.Message)
                   .HasMaxLength(1000);

            builder.HasOne(e => e.OfferedProduct)
                   .WithMany() // لا تربطه بمجموعة Requests معينة داخل Product
                   .HasForeignKey(e => e.OfferedProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.RequestedProduct)
                   .WithMany()
                   .HasForeignKey(e => e.RequestedProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
