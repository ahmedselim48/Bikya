
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
        private readonly IProductImageRepository productImageRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProductImageService productImageService;
        public ProductService(IProductRepository productRepository,
            IProductImageRepository productImageRepository, UserManager<ApplicationUser> userManager, ProductImageService productSeproductImageServicervice)
        {
            _productRepository = productRepository;
            this.productImageRepository = productImageRepository;
            this.userManager = userManager;
            this.productImageService = productImageService;
        }
        #endregion

        #region User Exists
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await userManager.Users.AnyAsync(u => u.Id == userId);
        }
        public async Task<bool> ISAdmin(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var roles = await userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        #endregion

        #region GET Methods
        public Task<IEnumerable<Product>> GetApprovedProductsWithImagesAsync()
        {
            return _productRepository.GetApprovedProductsWithImages();
        }

        public Task<IEnumerable<Product>> GetNotApprovedProductsWithImagesAsync()
        {
            return _productRepository.GetNotApprovedProductsWithImages();
        }

        public Task<IEnumerable<Product>> GetAllProductsWithImagesAsync()
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

        public async Task<IEnumerable<Product>> GetNotApprovedProductByUserAsync(int userId)
        {
            var iseExits = await UserExistsAsync(userId);
            if (!iseExits) throw new ArgumentException("User does not exist");

            return await _productRepository.GetNotApprovedProductByUserAsync(userId);
        }
        #endregion

        #region CRUD
        public async Task<Product> CreateProductAsync(ProductDTO productDTO, int userId)
        {
            bool exists = await _productRepository.GetSameProductSameUserAsync(userId, productDTO.Title);

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
                CategoryId = productDTO.CategoryId,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false,
                Status = Data.Enums.ProductStatus.Available,
                UserId = userId

            };

            await _productRepository.CreateAsync(product);
            return product;
        }
        public async Task UpdateProductAsync(int id,ProductDTO productDTO,int userId)
        {
            //automaper here
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) throw new ArgumentException("Product not found");

            var isAdmin = await ISAdmin(userId);

            if (existing.UserId != userId && !isAdmin) throw new UnauthorizedAccessException("You do not have permission to update this product");

            if(existing.Status != Data.Enums.ProductStatus.Available)
                throw new InvalidOperationException("You can not update a product that is in Process");

            existing.Title = productDTO.Title;
            existing.Description = productDTO.Description;
            existing.Price = productDTO.Price;
            existing.IsForExchange = productDTO.IsForExchange;
            existing.Condition = productDTO.Condition;
            existing.CategoryId = productDTO.CategoryId;
            existing.IsApproved = false;


            await _productRepository.UpdateAsync(existing);

            return ;
        }
  

        



        public async Task DeleteProductAsync(int id,int userId,string rootPath)
        {
            var existing = await _productRepository.GetProductWithImagesByIdAsync(id);
            if (existing == null) throw new ArgumentException("Product not found");
            var isAdmin = await ISAdmin(userId);

            if (existing.UserId != userId&&!isAdmin) throw new UnauthorizedAccessException("You do not have permission to delete this product");

            if (existing.Status != Data.Enums.ProductStatus.Available)
                throw new InvalidOperationException("You can not update a product that is in Process");

            //foreach (var image in existing.Images)
            //{
                
            //    await productImageService.DeleteProductImageAsync(image.Id,userId,rootPath);
            //}
            await _productRepository.DeleteAsync(existing);
            return ;
        }


        public async Task ApproveProductAsync(int productId)
        {
            var product = await _productRepository.GetProductWithImagesByIdAsync(productId);
            if (product == null) throw new ArgumentException("Product  not found");

            if (product.IsApproved) throw new InvalidOperationException("Product is already approved");
            await _productRepository.ApproveProductAsync(productId);
            return ;
        }
        public async Task RejectProductAsync(int productId)
        {
            var product = await _productRepository.GetProductWithImagesByIdAsync(productId);
            if (product == null) throw new ArgumentException("Product  not found");

            if (product.IsApproved) throw new InvalidOperationException("Product is already approved");
            await _productRepository.RejectProductAsync(productId);
            return;
        }
        #endregion

        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            //var category = = await _productRepository.GetCategoryByIdAsync(categoryId);
            //if (category == null) return Task.FromResult<IEnumerable<Product>>(null);
            var category = new Category(
                ); 
            return _productRepository.GetProductsByCategoryAsync(category);
        }







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

