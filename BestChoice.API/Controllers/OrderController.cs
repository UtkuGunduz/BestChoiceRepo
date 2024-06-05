using AutoMapper;
using BestChoice.API.Dtos;
using BestChoice.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BestChoice.API.Controllers
{

    [Route("api/Orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrders")]
        public List<OrderDto> GetOrders()
        {
            var orders = _context.Orders.ToList();
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return orderDtos;
        }

        [Authorize]
        [HttpGet("GetOrdersByUser/{userId}")]
        public List<OrderDto> GetOrdersByUser(string userId)
        {
            // Implement logic to retrieve orders for the specified user ID (consider authorization)
            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return orderDtos;
        }

        [Authorize]
        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // Find the order by ID (similar logic as before)
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Sipariş bulunamadı."); // Or a custom NotFound object
            }

            // Map the order to OrderDto
            var orderDto = _mapper.Map<OrderDto>(order);


            // Include order details if necessary (e.g., ordered products and quantities)
            // You might need to implement a separate endpoint or modify OrderDto to include details

            return Ok(orderDto);
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public ResultDto CreateOrder(OrderDto dto)
        {
            var result = new ResultDto();

            // Validate order data (e.g., product availability, user balance)
            // Implement validation logic here

            if (!ModelState.IsValid) // Check for validation errors
            {
                result.Status = false;
                result.Message = "Sipariş oluşturulamadı. Lütfen validation hatalarını kontrol edin.";
                return result;
            }

            // Map the OrderDto to Order model
            var order = _mapper.Map<Order>(dto);

            // Set additional properties (e.g., order date)
            order.OrderDate = DateTime.Now;

            // Add the order to the database context
            _context.Orders.Add(order);

            // Save changes to the database
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Sipariş başarıyla oluşturuldu.";
            return result;
        }

        [Authorize]
        [HttpPut("UpdateOrder/{id}")]
        public ResultDto UpdateOrder(int id, OrderDto dto)
        {
            var result = new ResultDto();

            // Check if order ID and DTO ID match
            if (id != dto.Id)
            {
                result.Status = false;
                result.Message = "İstek gövdesi ve id uyuşmuyor.";
                return result;
            }

            // Find the order by ID
            var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (orderToUpdate == null)
            {
                result.Status = false;
                result.Message = "Sipariş bulunamadı.";
                return result;
            }

            // Validate order data (e.g., product availability) on update if necessary

            // Map the updated properties from DTO to the existing order object
            _mapper.Map(dto, orderToUpdate);

            // Update the order in the database
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Sipariş başarıyla güncellendi.";
            return result;
        }

        [Authorize]
        [HttpDelete("DeleteOrder/{id}")]
        public ResultDto DeleteOrder(int id)
        {
            var result = new ResultDto();

            // Find the order by ID
            var orderToDelete = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (orderToDelete == null)
            {
                result.Status = false;
                result.Message = "Sipariş bulunamadı.";
                return result;
            }

            // Validate if the order can be deleted (e.g., not already shipped or completed)

            // Remove the order from the database
            _context.Orders.Remove(orderToDelete);

            // Save changes to the database
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Sipariş başarıyla silindi.";
            return result;
        }
    }
}