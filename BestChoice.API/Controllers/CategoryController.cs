using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BestChoice.API.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public CategoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetList")]
        public List<CategoryDto> GetList()
        {
            var categories = _context.Categories.ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return categoryDtos;
        }

        [HttpGet("Get/{id}")]
        public CategoryDto Get(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        [HttpGet("GetActiveCategories")]
        public List<CategoryDto> GetActiveCategories()
        {
            var categories = _context.Categories
                .Where(c => c.IsActive == true) // Explicit check for active categories
                .ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return categoryDtos;
        }

        [HttpGet("GetProductsWithinCategory/{id}")]
        public List<ProductDto> GetProductsWithinCategory(int id)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Visuals)
                .Where(p => p.IsActive == true && p.CategoryId == id) // Explicit checks
                .ToList();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return productDtos;
        }

        [HttpPost("AddCategory")]
        [Authorize(Roles = "Admin")]
        public ResultDto AddCategory(CategoryDto dto)
        {
            if (_context.Categories.Any(c => c.Name == dto.Name))
            {
                result.Status = false;
                result.Message = "Girilen Kategori Adı Kayıtlıdır!";
                return result;
            }

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Kategori Eklendi";
            return result;
        }

        [HttpPut("EditCategory")]
        [Authorize(Roles = "Admin")]
        public ResultDto EditCategory(CategoryDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == dto.Id);
            if (category == null)
            {
                result.Status = false;
                result.Message = "Kategori Bulunamadı!";
                return result;
            }


            category.Name = dto.Name;

            _context.Categories.Update(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Kategori Düzenlendi";
            return result;
        }

        [HttpPut("UpdateCategoryIsActive/{id}")]
        [Authorize(Roles = "Admin")]
        public ResultDto UpdateCategoryIsActive(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                result.Status = false;
                result.Message = "Kategori Bulunamadı!";
                return result;
            }

            category.IsActive = !category.IsActive;

            _context.Categories.Update(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = category.IsActive ? "Kategori Aktifleştirildi" : "Kategori Deaktifleştirildi";
            return result;
        }

        [HttpDelete("RemoveCategory/{id}")]
        [Authorize(Roles = "Admin")]
        public ResultDto RemoveCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                result.Status = false;
                result.Message = "Kategori Bulunamadı!";
                return result;
            }

            // Check if any products belong to this category before deleting
            var productsInCategory = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (productsInCategory.Count > 0)
            {
                result.Status = false;
                result.Message = "Kategoriye Ait Ürünler Var! Kategori Silinemez";
                return result;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Kategori Silindi";
            return result;
        }
    }
}