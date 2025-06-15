using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>

    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
           builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating).IsRequired();

            builder.Property(x => x.Comment).HasMaxLength(1000);

            builder.Property(x => x.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Seller)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Reviewer)
                .WithMany(u => u.ReviewsWritten)
                .HasForeignKey(x => x.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x=>x.Product)
                .WithMany(x=> x.Reviews)
                .HasForeignKey(x=>x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Order)
       .WithMany(o => o.Reviews)
       .HasForeignKey(x => x.OrderId)
       .OnDelete(DeleteBehavior.Cascade);










        }
    }
}
