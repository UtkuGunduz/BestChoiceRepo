using AutoMapper;
using BestChoice.API.Models;
using BestChoice.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BestChoice.API.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ResultDto result = new ResultDto();

        public ProductController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetProductList")]
        public List<ProductDto> GetProductList()
        {
            var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Discount)
            .ToList();

            var productModels = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Rating = p.Rating,
                Created = p.Created,
                Updated = p.Updated,
                AdditionalFeatures = p.AdditionalFeatures,
                IsActive = p.IsActive,
                ReturnPolicy = p.ReturnPolicy,

                CategoryName = p.Category.Name,
                DiscountAmount = p.Discount.DiscountAmount,
                DiscountName = p.Discount.Name,
                StockQuantity = p.StockQuantity,

            }).ToList();

            return productModels;
        }

        [HttpGet("Get/{id}")]
        public ActionResult<ProductDto> Get(int id)
        {
            var product = _context.Products.FirstOrDefault(s => s.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        [HttpPost("AddProduct")]
        public ActionResult<ResultDto> AddProduct(ProductDto dto)
        {
            if (_context.Products.Any(c => c.Name == dto.Name))
            {
                result.Status = false;
                result.Message = "Girilen Ürün Adı Kayıtlıdır!";
                return result;
            }

            var product = _mapper.Map<Product>(dto);

            product.Updated = DateTime.Now;
            product.Created = DateTime.Now;

            _context.Products.Add(product);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün Eklendi";
            return result;
        }

        [HttpPut("UpdateProduct")]
        public ActionResult<ResultDto> UpdateProduct(ProductDto dto, IFormFile file)
        {
            var product = _context.Products.FirstOrDefault(s => s.Id == dto.Id);
            var visual = _context.Visuals.FirstOrDefault(v => v.Id == dto.Id);
            if (product == null)
            {
                result.Status = false;
                result.Message = "Ürün Bulunamadı!";
                return result;
            }

            product.Name = dto.Name;
            product.IsActive = dto.IsActive;
            product.Price = dto.Price;
            product.Updated = DateTime.Now;

            if (product.Category != null)
            {
                product.CategoryId = product.Category.Id;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün Düzenlendi";
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveProduct/{id}")]
        public ActionResult<ResultDto> RemoveProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(s => s.Id == id);
            if (product == null)
            {
                result.Status = false;
                result.Message = "Ürün Bulunamadı!";
                return result;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün Silindi";
            return result;
        }

        [HttpGet("Search")]
        public async Task<ActionResult> Search(string term)
        {
            var routeList = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(term))
            .Take(5)
            .Select(p => new { id = p.Id, label = p.Name, name = "ProductID", category = p.Category.Name })
            .ToListAsync();
            return Ok(routeList);
        }

        [HttpGet("GetByCategory/{categoryId}")]
        public List<ProductDto> GetByCategory(int categoryId)
        {
            var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Discount)
            .Where(p => p.CategoryId == categoryId)
            .ToList();

            var productModels = _mapper.Map<List<ProductDto>>(products);
            return productModels;
        }

        [HttpGet("GetActiveProducts")]
        public List<ProductDto> GetActiveProducts()
        {
            var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Discount)
            .Include(p => p.Visuals)
            .ToList();

            var activeProducts = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Rating = p.Rating,
                Created = p.Created,
                Updated = p.Updated,
                AdditionalFeatures = p.AdditionalFeatures,
                IsActive = p.IsActive,
                ReturnPolicy = p.ReturnPolicy,

                CategoryName = p.Category.Name,
                DiscountAmount = p.Discount.DiscountAmount,
                DiscountName = p.Discount.Name,

            }).ToList();

            var productModels = _mapper.Map<List<ProductDto>>(activeProducts);
            return productModels;
        }

        [HttpGet("GetNewProducts")]
        public List<ProductDto> GetNewProducts()
        {
            var currentDate = DateTime.Now;
            var thirtyDaysAgo = currentDate.AddDays(-30);

            var newProducts = _context.Products
            .Include(p => p.Visuals)
            .Where(p => p.Created >= thirtyDaysAgo)
            .ToList();
            var productModels = _mapper.Map<List<ProductDto>>(newProducts);

            return productModels;
        }

        [HttpGet("GetProductDetails/{id}")]
        public ActionResult<ProductDetailsDto> GetProductDetails(int id)
        {
            var product = _context.Products
            .Include(p => p.Category)

            .Include(p => p.Discount)
            .Include(p => p.Visuals)
            .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDetails = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                // ... map other properties from Product to ProductDetailsDto
                Category = _mapper.Map<CategoryDto>(product.Category),
                Discount = _mapper.Map<DiscountDto>(product.Discount),
                Visuals = product.Visuals.Select(v => new VisualDto // Map Visuals to VisualDto
                {
                    // Map properties from Visual to VisualDto
                }).ToList()
            };
            return productDetails;
        }

        [HttpGet("GetFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] ProductFilter filter)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .Where(p =>
                    (filter.PriceMin == null || p.Price >= filter.PriceMin) &&
                    (filter.PriceMax == null || p.Price <= filter.PriceMax) &&
                    (filter.CategoryId == null || p.CategoryId == filter.CategoryId) &&
                    (filter.DiscountId == null || p.DiscountId == filter.DiscountId)
                );

            // Use AutoMapper if available
            if (_mapper != null)
            {
                var productDtos = await _mapper.ProjectTo<ProductDto>(products).ToListAsync();
                return Ok(productDtos);
            }

            var productModels = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Rating = p.Rating,
                Created = p.Created,
                Updated = p.Updated,
                AdditionalFeatures = p.AdditionalFeatures,
                IsActive = p.IsActive,
                ReturnPolicy = p.ReturnPolicy,

                CategoryName = p.Category.Name,
                DiscountAmount = p.Discount != null ? p.Discount.DiscountAmount : null,
                DiscountName = p.Discount != null ? p.Discount.Name : null,
                StockQuantity = p.StockQuantity,
            }).ToList();

            return Ok(productModels);
        }

        [HttpGet("GetProductStock/{id}")]
        public ActionResult<int> GetProductStock(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return product.StockQuantity;
        }

        [HttpPut("UpdateProductStock/{id}/{quantity}")]
        public ActionResult<ResultDto> UpdateProductStock(int id, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                result.Status = false;
                result.Message = "Ürün Bulunamadı!";
                return result;
            }

            product.StockQuantity = quantity;
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün Stoğu Güncellendi";
            return result;
        }

        [HttpPost("AddProductReview/{id}")]
        public ActionResult<ResultDto> AddProductReview(int id, ReviewDto reviewDto)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                result.Status = false;
                result.Message = "Ürün Bulunamadı!";
                return result;
            }

            var review = new Review
            {
                ProductId = id,
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                Created = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün Yorumu Eklendi";
            return result;
        }

        [HttpGet("GetProductReviews/{id}")]
        public List<ReviewDto> GetProductReviews(int id)
        {
            var reviews = _context.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == id)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.Rating,
                Created = r.Created,
                UserName = r.User.UserName
            })
                .ToList();

            return reviews;
        }
    }
}