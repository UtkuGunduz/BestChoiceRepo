using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using System.Data;


namespace BestChoice.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // User Management
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserEditDto>().ReverseMap();
            CreateMap<AppRole, RoleDto>().ReverseMap();
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<LoginDto, AppUser>().ReverseMap();

            // E-Commerce
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDetailsDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();

            // Other DTOs
            CreateMap<WishlistItem, WishlistItemDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartSummaryDto, Cart>().ReverseMap();
            CreateMap<ChangePasswordDto, AppUser>().ReverseMap();
        }
    }
}
