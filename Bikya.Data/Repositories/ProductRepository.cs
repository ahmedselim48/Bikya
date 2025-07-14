using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikya.Data.Models;
using Bikya.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Bikya.Data.Repositories.Interfaces;


namespace Bikya.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BikyaContext context;

        public ProductRepository(BikyaContext context)
        {
            this.context = context;
        }

        #region Test

        public async Task<bool> GetSameProductSameUserAsync(int userId, string title)
        {

            return await context.Products.AnyAsync(p => p.UserId == userId && p.Title == title);

        }
        #endregion


        #region Get

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithImages()
        {
            return await context.Products.Include(p => p.Images).ToListAsync();


        }
        public async Task<IEnumerable<Product>> GetApprovedProductsWithImages()
        {


            return await context.Products.Include(p => p.Images).Where(p => p.IsApproved == true).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetNotApprovedProductsWithImages()
        {

            return await context.Products.Include(p => p.Images).Where(p => p.IsApproved == false).ToListAsync();


        }

        public async Task<IEnumerable<Product>> GetProductsByUserAsync(int userId)
        {

            return await context.Products.Include(p => p.Images).Where(p => p.UserId == userId && p.IsApproved == true).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetNotApprovedProductByUserAsync(int userId)
        {

            return await context.Products.Include(p => p.Images).Where(p => p.UserId == userId && p.IsApproved == false).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Category category)
        {

            return await context.Products
                .Where(p => p.CategoryId == category.Id&&p.IsApproved==true)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

     


        public async Task<Product> GetProductWithImagesByIdAsync(int id)
        {
            return await context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }



        #endregion

        public async Task CreateAsync(Product product)
        {
            context.Products.Add(product);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            context.Products.Update(product);
            await SaveChangesAsync();
       
        }

        public async Task DeleteAsync(Product product)
        {
                context.Products.Remove(product);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Database update failed: " + inner, ex);
            }
        }
      



        public async Task ApproveProductAsync(int id)
        {
            var product = await GetByIdAsync(id);
            product.IsApproved = true;
            await SaveChangesAsync();

        }
        public async Task RejectProductAsync(int id)
        {
            var product = await GetByIdAsync(id);
            DeleteAsync(product);
              await SaveChangesAsync();
            return;
        }


        //public Task<IEnumerable<Product>> GetProductsByConditionAsync(string condition)
        //{
        //    return _productRepository.GetProductsByConditionAsync(condition);
        //}
        //public Task<IEnumerable<Product>> GetProductsForExchangeAsync()
        //{
        //    return _productRepository.GetProductsForExchangeAsync();
        //}

        //public Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        //{
        //    return _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
        //}
        //public Task<IEnumerable<Product>> GetTopRatedProductsAsync(int count)
        //{
        //    return _productRepository.GetTopRatedProductsAsync(count);
        //}
        //public Task<IEnumerable<Product>> GetLatestProductsAsync(int count)
        //{
        //    return _productRepository.GetLatestProductsAsync(count);
        //}
        //public Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        //{
        //    return _productRepository.GetFeaturedProductsAsync();
        //}

        //public Task<IEnumerable<Product>> GetProductsByTitleAsync(string title)
        //{
        //    return _productRepository.GetProductsByTitleAsync(title);
        //}
        //public Task<IEnumerable<Product>> GetProductsByDescriptionAsync(string description)
        //{
        //    return _productRepository.GetProductsByDescriptionAsync(description);
        //}
        //public Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal price)
        //{
        //    return _productRepository.GetProductsByPriceAsync(price);
        //}
        //public Task<IEnumerable<Product>> GetProductsByIsForExchangeAsync(bool isForExchange)
        //{
        //    return _productRepository.GetProductsByIsForExchangeAsync(isForExchange);
        //}
        //public Task<IEnumerable<Product>> GetProductsByCreatedAtAsync(DateTime createdAt)
        //{
        //    return _productRepository.GetProductsByCreatedAtAsync(createdAt);
        //}
    }
}
