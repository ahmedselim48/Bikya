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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            
            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.PlatformFee)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.SellerAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

          
            builder.Property(o => o.Status)
                   .IsRequired();

            builder.Property(o => o.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(o => o.PaidAt)
                   .IsRequired(false);

            builder.Property(o => o.CompletedAt)
                   .IsRequired(false);

            builder.HasOne(o => o.Product)
                   .WithMany()
                   .HasForeignKey(o => o.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Order -> Buyer
            builder.HasOne(o => o.Buyer)
                .WithMany(u => u.OrdersBought)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths

            // Order -> Seller
            builder.HasOne(o => o.Seller)
                .WithMany(u => u.OrdersSold)
                .HasForeignKey(o => o.SellerId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths


        }
    }
}
