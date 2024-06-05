using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BestChoice.API.Controllers
{
    [Route("api/Wishlist")]
    [Authorize]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        ResultDto result = new ResultDto();

        public WishlistController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetUserWishlist")]
        public List<WishlistItemDto> GetUserWishlist()
        {
            var userId = _userManager.GetUserId(User);
            var userWishlistItems = _context.WishlistItems
                    .Include(w => w.Product)
                    .ThenInclude(p => p.Visuals)
                    .Where(w => w.UserId == userId)
                    .ToList();
            var wishlistItemDtos = _mapper.Map<List<WishlistItemDto>>(userWishlistItems);
            return wishlistItemDtos;
        }

        [HttpPost]
        [Route("AddToUserWishlist")]
        public async Task<IActionResult> AddToUserWishlist(int productId)
        {
            var userId = _userManager.GetUserId(User);

            // Check if product already exists in wishlist
            if (_context.WishlistItems.Any(w => w.UserId == userId && w.ProductId == productId))
            {
                return BadRequest("Product already exists in wishlist");
            }

            // Create a new WishlistItem
            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ProductId = productId
            };

            // Add the WishlistItem to the database
            await _context.WishlistItems.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok("Product added to wishlist");
        }

        [HttpDelete]
        [Route("RemoveFromUserWishlist/{id}")]
        public ResultDto RemoveFromUserWishlist(int id)
        {
            var userId = _userManager.GetUserId(User);
            var wishlistItem = _context.WishlistItems.FirstOrDefault(item => item.Id == id && item.UserId == userId);
            if (wishlistItem == null)
            {
                result.Status = false;
                result.Message = "Ürün favorilerde bulunamadı!";
                return result;
            }
            _context.WishlistItems.Remove(wishlistItem);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Ürün başarıyla favorilerden kaldırıldı";
            return result;
        }
    }
}