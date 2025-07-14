using Bikya.Data.Models;
using Bikya.Data.Repositories.Interfaces;
using Bikya.DTOs.ProductDTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bikya.Services.Services 
{ 
    public class ProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> userManager;


        public ProductImageService(IProductImageRepository productImageRepository, IProductRepository productRepository, UserManager<ApplicationUser> userManager)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            this.userManager = userManager;
        }

        public async Task<bool> ISAdmin(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var roles = await userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        public async Task AddProductImageAsync(ProductImageDTO productImageDTO,string rootPath)
        {
            var product = await _productRepository.GetByIdAsync(productImageDTO.ProductId);
            if (product == null) throw new ArgumentNullException("Product not found");


            var fileName = $"{Guid.NewGuid()}_{productImageDTO.Image.FileName}";
            var folderPath = Path.Combine(rootPath, "Images", "Products");

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productImageDTO.Image.CopyToAsync(stream); 
            }

            var image = new ProductImage
            {
                ProductId = productImageDTO.ProductId,
                ImageUrl = $"/Images/Products/{fileName}",
                IsMain = productImageDTO.IsMain,
                CreatedAt= DateTime.UtcNow
            };
 

            await _productImageRepository.CreateAsync(image);
        }
        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new ArgumentException("Product not found");

            return await _productImageRepository.GetByProductIdAsync(productId);
        }

        public async Task<ProductImage> GetImageByIdAsync(int id)
        {
            var image = await _productImageRepository.GetByIdAsync(id);
            if (image == null)
                throw new ArgumentException("Product image not found");

            return image;
        }

        public async Task UpdateProductImageAsync(ProductImage productImage)
        {
            if (productImage == null)
                throw new ArgumentNullException(nameof(productImage));

            if (string.IsNullOrWhiteSpace(productImage.ImageUrl))
                throw new ArgumentException("Image URL cannot be null or empty");

            var product = await _productRepository.GetByIdAsync(productImage.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found", nameof(productImage.ProductId));

            await _productImageRepository.UpdateAsync(productImage);
        }

        public async Task DeleteProductImageAsync(int id,int userId,string rootPath)
        {
            var productImage = await _productImageRepository.GetByIdAsync(id);
            if (productImage == null)
                throw new ArgumentException("Product image not found", nameof(id));
            var product = await _productRepository.GetByIdAsync(productImage.ProductId);
            if(product == null)
                throw new ArgumentException("Product not found");

            var isAdmin = await ISAdmin(userId);
            if (product.UserId != userId&&!isAdmin)
                throw new UnauthorizedAccessException("You do not have permission to delete this product image");

            if (product.Status != Data.Enums.ProductStatus.Available)
                throw new InvalidOperationException("You can not update a product that is in Process");

            //var fileName = productImage.ImageUrl.TrimStart('/');
            //var filePath = Path.Combine(rootPath,fileName);

            //if (System.IO.File.Exists(filePath))
            //{
            //    System.IO.File.Delete(filePath);
            //}


            await _productImageRepository.DeleteAsync(productImage);
        }
    }
}