using Bikya.Data.Models;
using Bikya.Data.Repositories.Interfaces;
using Bikya.Data.Response;
using Bikya.DTOs.ProductDTO;
using Bikya.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;


namespace Bikya.API.Areas.Products.Controller
{

    [Area("Products")]
    [Route("api/[area]/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        #region Services
        private readonly ProductService productService;
        private readonly ProductImageService productImageService;
         private readonly IWebHostEnvironment env;
        public ProductController(IWebHostEnvironment env ,ProductService productService, ProductImageService productImageService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
            this.env = env;
        }

        #endregion

        # region GET
        [HttpGet("AllProducts")]
        public async Task<IActionResult> GetAllProductsWithImages()
        {
            try
            {
                var products = await productService.GetProductsWithImagesAsync();

                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products));
            }catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("server error", 500, new List<string> { ex.Message }));
            }



        }

        [HttpGet("AllProducts/{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await productService.GetProductWithImagesByIdAsync(id);

                return Ok(ApiResponse<Product>.SuccessResponse(product));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message,404));
            }
            catch (Exception ex)
            {
                return StatusCode(500,ApiResponse<string>.ErrorResponse("server error",500, new List<string> { ex.Message }));
            }
            
        }


        [HttpGet("UserProduct/{userId}")]
        public async Task<IActionResult> GetProductByUserAsync(int userId)
        {
            try
            {
                var products = await productService.GetProductsByUserAsync(userId);
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products));
            }
            catch (ArgumentException ex) { return NotFound(ApiResponse<string>.ErrorResponse(ex.Message, 404)); }
            catch(Exception ex) { return StatusCode(500, ApiResponse<string>.ErrorResponse("server error", 500, new List<string> { ex.Message })); }

        }

 

        //optional: maybe in CategoryProduct
        [HttpGet("CategoryProduct/{id}")]
        public async Task<IActionResult> GetProductsByCategoryAsync(int id)
        {
            //var product = await productService.GetProductsByCategoryAsync(id);
            //if (product == null)
            //{
            //    return NotFound(new { Message = "Product not found" });
            //}
            //return Ok(product);
            return Ok();
        }

        #endregion







        #region CRUD Products
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductDTO product)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
               await productService.CreateProductAsync(product,userId);
                return Ok(ApiResponse<bool>.SuccessResponse(true));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProductc(int id, [FromBody] ProductDTO product)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                await productService.UpdateProductAsync(id, product, userId);
                return Ok(ApiResponse<bool>.SuccessResponse(true));
            }
            catch (ArgumentException ex )
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message, 404));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }
        
          
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await productService.DeleteProductAsync(id, userId);
                return Ok(ApiResponse<bool>.SuccessResponse(true));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message, 404));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }


        }


        #endregion


        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("AddWithImages")]
        public async Task<IActionResult> CreateProductWithImagesAsync([FromForm] CreateProductWithimagesDTO productDTO)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var product = new ProductDTO
                {
                    Title = productDTO.Title,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    IsForExchange = productDTO.IsForExchange,
                    Condition = productDTO.Condition,
                    //CategoryId = productDTO.CategoryId
                };
                var createdProduct =await productService.CreateProductAsync(product, userId);
                var rootPath = env.WebRootPath;
                if (productDTO.MainImage != null) {
                    await productImageService.AddProductImageAsync(new ProductImageDTO
                    {
                        ProductId = createdProduct.Id,
                        Image = productDTO.MainImage,
                        IsMain = true
                    },rootPath);
                }

                if (productDTO.AdditionalImages.Any())
                {
                    foreach (var image in productDTO.AdditionalImages)
                    {
                        await productImageService.AddProductImageAsync(new ProductImageDTO
                        {
                            ProductId = createdProduct.Id,
                            Image = image,
                            IsMain = false
                        }, rootPath);
                    }
                }
                return Ok(ApiResponse<bool>.SuccessResponse(true));

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }
        }

        #region IMAGES

    
        [Authorize]
        [HttpPost("Product/{productId}/AddImage")]
        public async Task<IActionResult> AddImageAsync(int productId, [FromForm] CreateImageDTO createImageDTO)
        {
            try
            {
                var rootPath = env.WebRootPath;
                if (createImageDTO.Image != null)
                {
                    await productImageService.AddProductImageAsync(new ProductImageDTO
                    {
                        ProductId = productId,
                        Image = createImageDTO.Image,
                        IsMain = createImageDTO.IsMain
                    }, rootPath);
                }


                return Ok(ApiResponse<bool>.SuccessResponse(true));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }
        }

        [HttpDelete("DeleteImage/{id}")]
        public async Task<IActionResult> DeleteProductImageAsync(int productId,int id)
        {
            try
            {
                await productImageService.DeleteProductImageAsync(id);
                return Ok(ApiResponse<bool>.SuccessResponse(true));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error", 500, new List<string> { ex.Message }));
            }

        }

        #endregion
    }
}
        //GetProductsByCategoryAsync

        //public async Task<IActionResult> GetAllProducts()
        //{
        //        var products = await productService.GetAllProductsAsync();
        //        return Ok(products);
            
      
        //}

        //[HttpGet("AllProducts/{id}")]
        //public async Task<IActionResult> GetProductByIdAsync(int id)
        //{
        //    var product = await productService.GetProductByIdAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound(new { Message = "Product not found" });
        //    }
        //    return Ok(product);
        //}




        //[HttpPut("UpdateImages/{id}")]
        //public async Task<IActionResult> UpdateProductImageAsync(int id, [FromBody] ProductImage productImage)
        //{
        //    if (productImage == null)
        //    {
        //        return BadRequest(new { Message = "Invalid product image data" });
        //    }
        //    try
        //    {
        //        await productImageService.UpdateProductImageAsync(productImage);
        //        return Ok("ok");
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }


        //}