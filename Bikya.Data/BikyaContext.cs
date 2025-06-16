using Bikya.Data.Configurations;
using Bikya.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data
{
    public class BikyaContext : DbContext
    {
        public BikyaContext(DbContextOptions<BikyaContext> options)
            : base(options) 
        { 
        
        }

        public DbSet<ShippingInfo> ShippingInfos { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Category> categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ShippingInfoConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
       
  base.OnModelCreating(modelBuilder);
        }
    }
}
