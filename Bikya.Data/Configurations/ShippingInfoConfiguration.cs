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
    public class ShippingInfoConfiguration : IEntityTypeConfiguration<ShippingInfo>
    {
        //  public void Configure(EntityTypeBuilder<Review> builder)
       public  void Configure(EntityTypeBuilder<ShippingInfo> builder)
        {
            builder.HasKey(x => x.ShippingId);

            builder.Property(x => x.RecipientName)
                .IsRequired().
                HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(7);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(7);

            builder.Property(x => x.status).HasConversion<string>()
                .HasDefaultValue(ShippingStatus.Pending);

            builder.Property(x =>x.CreateAt)
       .HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(x=>x.Order)
                .WithOne(x=> x.ShippingInfo)
                .HasForeignKey<ShippingInfo>(x=> x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            







        }
    }
}
