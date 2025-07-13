using Bikya.Data.Models;
using Bikya.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikya.Data.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly BikyaContext context;
        public ProductImageRepository(BikyaContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(ProductImage product)
        {
            context.ProductImages.Add(product);
           await SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductImage product)
        {
             context.ProductImages.Remove(product);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync()
        {
            return await context.ProductImages.ToListAsync();
        }

     
        public async Task<ProductImage> GetByIdAsync(int id)
        {
            return await context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task UpdateAsync(ProductImage productImage)
        {
          context.ProductImages.Update(productImage);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await context.ProductImages.Include(pi => pi.Product).Where(pi => pi.ProductId == productId).ToListAsync();
        }
    }
}
