using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BestChoice.API.Controllers
{
    [Route("api/Coupons")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public CouponController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetCoupons")]
        [Authorize(Roles = "Admin")]
        public List<CouponDto> GetCoupons()
        {
            var coupons = _context.Coupons.ToList();
            var couponDtos = _mapper.Map<List<CouponDto>>(coupons);
            return couponDtos;
        }

        [HttpGet("GetActiveCoupons")]
        public List<CouponDto> GetActiveCoupons()
        {
            var coupons = _context.Coupons
                .Where(c => c.IsValid && c.ExpiryDate >= DateTime.Now) // Filter for active & valid coupons
                .ToList();
            var couponDtos = _mapper.Map<List<CouponDto>>(coupons);
            return couponDtos;
        }

        [HttpPost("AddCoupon")]
        [Authorize(Roles = "Admin")]
        public ResultDto AddCoupon(string code, double discountPercentage, DateTime expirationDate)
        {
            // Existing logic for adding a coupon...

            return result;
        }

        [HttpPut("UpdateCoupon/{id}")]
        [Authorize(Roles = "Admin")]
        public ResultDto UpdateCoupon(int id, CouponDto dto)
        {
            // Existing logic for updating a coupon...

            return result;
        }

        [HttpDelete("DeleteCoupon/{id}")]
        [Authorize(Roles = "Admin")]
        public ResultDto DeleteCoupon(int id)
        {
            // Existing logic for deleting a coupon...

            return result;
        }

        // Placeholder for ApplyCoupon endpoint (implement logic for validation and discount calculation)
        [HttpPost("ApplyCoupon")]
        public ResultDto ApplyCoupon(string code)
        {
            // Implement logic to validate coupon code and apply discount
            result.Status = false;
            result.Message = "Coupon functionality not yet implemented";
            return result;
        }

        [HttpPut("UpdateCouponIsActive/{id}")]
        [Authorize(Roles = "Admin")]
        public ResultDto UpdateCouponIsActive(int id, bool isActive)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == id);
            if (coupon == null)
            {
                result.Status = false;
                result.Message = "Kupon Bulunamadı!";
                return result;
            }

            coupon.IsValid = isActive;
            _context.Coupons.Update(coupon);
            _context.SaveChanges();

            result.Status = true;
            result.Message = isActive ? "Kupon Aktifleştirildi" : "Kupon Deaktifleştirildi";
            return result;
        }
    }
}