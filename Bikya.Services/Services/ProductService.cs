
using Bikya.Data.Models;
using Bikya.Data.Repositories;
using Bikya.Data.Repositories.Interfaces;
using Bikya.DTOs.ProductDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Bikya.Services.Services 
{
    public class ProductService
    {
        #region Repo 
        private readonly IProductRepository _productRepository;
        //private readonly ICategoryRepository _categoryRepository;
        private readonly IProductImageRepository productImageRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public ProductService(IProductRepository productRepository,
            //ICategoryRepository _categoryRepository,
            IProductImageRepository productImageRepository, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            //this._categoryRepository = _categoryRepository;
            this.productImageRepository = productImageRepository;
            this.userManager = userManager;
        }
        #endregion

        #region User Exists
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await userManager.Users.AnyAsync(u => u.Id == userId);
        }

        #endregion

        #region GET Methods
        public Task<IEnumerable<Product>> GetProductsWithImagesAsync()
        {
            return _productRepository.GetProductsWithImages();
        }

        public async Task<Product> GetProductWithImagesByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductWithImagesByIdAsync(productId);
            if (product == null) throw new ArgumentException("Product  not found");


            return product;
        }
                
        public async Task<IEnumerable<Product>> GetProductsByUserAsync(int userId)
        {
            var iseExits =await  UserExistsAsync(userId);
            if (!iseExits)  throw new ArgumentException("User does not exist");

            return await _productRepository.GetProductsByUserAsync(userId);
        }
        #endregion


        public async Task UpdateProductAsync(int id,ProductDTO productDTO,int userId)
        {
            //automaper here
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) throw new ArgumentException("Product not found");


            if (existing.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to update this product");

            existing.Title = productDTO.Title;
            existing.Description = productDTO.Description;
            existing.Price = productDTO.Price;
            existing.IsForExchange = productDTO.IsForExchange;
            existing.Condition = productDTO.Condition;
            //existing.CategoryId = productDTO.CategoryId;
            

            await _productRepository.UpdateAsync(existing);

            return ;
        }
        public async Task<Product> CreateProductAsync(ProductDTO productDTO, int userId)
        {
            bool exists = await _productRepository.GetSameProductSameUserAsync(userId,productDTO.Title);

            if (exists)
                throw new ArgumentException("You already added a product with this title");
            
            //automaper here
            var product = new Data.Models.Product
            {
                Title = productDTO.Title,
                Description = productDTO.Description,
                Price = productDTO.Price,
                IsForExchange = productDTO.IsForExchange,
                Condition = productDTO.Condition,
                //CategoryId = productDTO.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UserId = userId

            };

            await _productRepository.CreateAsync(product);
            return product;
        }

        



        public async Task DeleteProductAsync(int id,int userId)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) throw new ArgumentException("Product not found");


            if (existing.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to delete this product");
            await _productRepository.DeleteAsync(existing);
            return ;
        }

        

        //public Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId )
        //{
        //    var category=_categoryRepository.GetByIdAsync(categoryId);
        //    if (category == null) return Task.FromResult<IEnumerable<Product>>(null);
        //    return _productRepository.GetProductsByCategoryAsync(category);
        //}







        //public Task<Data.Models.Product> GetProductByIdAsync(int id)
        //{
        //    return _productRepository.GetByIdAsync(id);
        //}


        //public Task<IEnumerable<Data.Models.Product>> GetAllProductsAsync()
        //{
        //    return _productRepository.GetAllAsync();
        //}


    }

}

