using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BestChoice.API.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        ResultDto result = new ResultDto();

        public CartController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetUserCart")]
        public List<CartItemDto> GetUserCart()
        {
            var userId = _userManager.GetUserId(User);
            var userCartItems = _context.CartItems
                .Include(w => w.Product)
                .ThenInclude(p => p.Visuals)
                .Where(w => w.UserId == userId)
                .ToList();
            var cartItemDtos = _mapper.Map<List<CartItemDto>>(userCartItems);
            return cartItemDtos;
        }

        [HttpPost]
        [Route("AddToCart")]
        public ResultDto AddToCart(CartItemDto dto)
        {
            var userId = _userManager.GetUserId(User);
            dto.UserId = userId;
            var cartItem = _mapper.Map<CartItem>(dto);
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Ürün sepete eklendi";
            return result;
        }

        [HttpDelete]
        [Route("RemoveFromCart/{id}")]
        public ResultDto RemoveFromCart(int id)
        {
            var cartItem = _context.CartItems.FirstOrDefault(item => item.Id == id);
            if (cartItem == null)
            {
                result.Status = false;
                result.Message = "Ürün sepette bulunamadı!";
                return result;
            }

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Ürün başarıyla sepetten kaldırıldı";

            return result;
        }

        [HttpDelete]
        [Route("ClearCart")]
        public ResultDto ClearCart()
        {
            _context.CartItems.RemoveRange(_context.CartItems);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Sepet başarıyla temizlendi";

            return result;
        }

        [HttpPut]
        [Route("UpdateCartItem/{id}")]
        public ResultDto UpdateCartItem(int id, CartItemDto dto)
        {
            var cartItem = _context.CartItems.FirstOrDefault(c => c.Id == id);
            if (cartItem == null)
            {
                result.Status = false;
                result.Message = "Belirtilen öğe bulunamadı";
                return result;
            }

            cartItem.Quantity = dto.Quantity;
            _context.CartItems.Update(cartItem);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Öğe güncellendi";
            return result;
        }

        [HttpGet]
        [Route("GetTotalPrice")]
        public decimal GetTotalPrice()
        {
            decimal totalPrice = _context.CartItems
                .Where(c => c.Product != null && c.Product.Price.HasValue) // Combined check
                .Sum(c => c.Quantity * c.Product.Price.Value);

            return totalPrice;
        }

        [HttpGet]
        public int GetItemCount()
        {
            int? itemCount = _context.CartItems?.Sum(c => c.Quantity); // Sum might return null for empty CartItems
            return itemCount ?? 0; // Use ?? to handle potential null from Sum
        }

        [HttpPost]
        [Route("ApplyCoupon/{couponCode}")]
        public ResultDto ApplyCoupon(string couponCode)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Code == couponCode);

            if (coupon == null || !coupon.IsValid)
            {
                result.Status = false;
                result.Message = "Geçersiz kupon kodu";
                return result;
            }

            decimal discountedPrice = GetCartTotalWithDiscount(coupon);

            UpdateCartItemPrices(discountedPrice);

            result.Status = true;
            result.Message = "Kupon uygulandı";
            return result;
        }

        [HttpGet]
        [Route("GetCartTotalWithDiscount")]
        private decimal GetCartTotalWithDiscount(Coupon coupon)
        {
            decimal totalCartPrice = _context.CartItems
                .Where(c => c.Product != null && c.Product.Price.HasValue) // Filter for non-null Product and valid Price
                .Sum(c => c.Quantity * c.Product.Price.Value);

            double discountPercentage = coupon.DiscountPercentage;
            decimal discountAmount = totalCartPrice * (decimal)(discountPercentage / 100.0);
            decimal discountedPrice = totalCartPrice - discountAmount;
            return discountedPrice;
        }

        private void UpdateCartItemPrices(decimal discountedPrice)
        {
            var cartItems = _context.CartItems.ToList();
            foreach (var cartItem in cartItems)
            {
                var productPrice = cartItem.Product.Price;
                cartItem.Product.Price = discountedPrice * (productPrice / cartItem.Product.Price);
            }
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("GetUserCartDetails")]
        public List<CartItemDetailDto> GetUserCartDetails()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            var cartItemDetailsList = new List<CartItemDetailDto>();

            foreach (var cartItem in cartItems)
            {
                var cartItemDetailDto = new CartItemDetailDto
                {
                    Id = cartItem.Id,
                    UserId = cartItem.UserId,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product.Name,
                    ProductPrice = cartItem.Product.Price,
                    ProductStockQuantity = cartItem.Product.StockQuantity,
                    ProductImageUrls = cartItem.Product.Visuals.Select(v => v.PhotoUrl).ToList(),
                    ProductDescription = cartItem.Product.Description,
                    Quantity = cartItem.Quantity,
                    AddedAt = cartItem.AddedAt
                };

                cartItemDetailsList.Add(cartItemDetailDto);
            }

            return cartItemDetailsList;
        }

        [HttpGet]
        [Route("GetCartByUserId/{id}")]
        public List<CartItemDto> GetCartByUserId(int userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId.ToString()).ToList();
            var cartItemDtos = _mapper.Map<List<CartItemDto>>(cartItems);
            return cartItemDtos;
        }

        [HttpPut]
        [Route("UpdateCartItemQuantity/{id}")]
        public ResultDto UpdateCartItemQuantity(int id, int newQuantity)
        {
            var cartItem = _context.CartItems.FirstOrDefault(c => c.Id == id);
            if (cartItem == null)
            {
                result.Status = false;
                result.Message = "Belirtilen öğe bulunamadı";
                return result;
            }

            cartItem.Quantity = newQuantity;
            _context.CartItems.Update(cartItem);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Öğe miktarı güncellendi";
            return result;
        }

        [HttpGet]
        [Route("GetCartSummary")]
        public CartSummaryDto GetCartSummary()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            var cartSummary = new CartSummaryDto();
            cartSummary.ItemCount = cartItems.Sum(c => c.Quantity);
            cartSummary.TotalPrice = cartItems.Sum(c => (decimal)c.Quantity * (c.Product.Price ?? 0));
            cartSummary.DiscountedTotalPrice = GetCartTotalWithDiscount(null); // Calculate total with no coupon applied

            return cartSummary;
        }
    }
}