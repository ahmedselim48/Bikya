using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bikya.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Address)
                   .HasMaxLength(255);

            builder.Property(u => u.ProfileImageUrl)
                   .HasMaxLength(500);

            builder.Property(u => u.IsVerified)
                   .HasDefaultValue(false);

            builder.HasMany(u => u.Products)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ReviewsWritten)
                   .WithOne(r => r.Reviewer)
                   .HasForeignKey(r => r.ReviewerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReviewsReceived)
                   .WithOne(r => r.Seller)
                   .HasForeignKey(r => r.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.OrdersBought)
                   .WithOne(o => o.Buyer)
                   .HasForeignKey(o => o.BuyerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.OrdersSold)
                   .WithOne(o => o.Seller)
                   .HasForeignKey(o => o.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Wallet (1:1)
            builder.HasOne(u => u.Wallet)
                   .WithOne(w => w.User)
                   .HasForeignKey<Wallet>(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
