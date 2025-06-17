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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(u => u.Email)
              .IsRequired()
              .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(u => u.Role)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(u => u.Phone)
               .IsRequired()
               .HasMaxLength(12);

            builder.Property(u => u.Address)
              .IsRequired()
              .HasMaxLength(255);

            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

            builder.Property(u => u.IsVerified)
           .HasDefaultValue(false);

            builder.HasMany(u => u.Products)
              .WithOne(p => p.User)
              .HasForeignKey(p => p.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(u => u.Wallet)
            //    .WithOne(w => w.User)
            //    .HasForeignKey<Wallet>(w => w.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
