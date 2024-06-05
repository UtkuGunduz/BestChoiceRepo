using BestChoice.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestChoice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DropdownDataController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                                           .Select(c => new { c.Id, c.Name })
                                           .Distinct()
                                           .ToListAsync();
            if (!categories.Any())
            {
                return Ok(new List<object> { new { name = "Girilmemiş", id = 0 } });
            }
            return Ok(categories);
        }


        [HttpGet("GetDiscounts")]
        public async Task<IActionResult> GetDiscounts()
        {
            var discounts = await _context.Discounts
                                          .Select(d => new { d.Id, d.Name })
                                          .Distinct()
                                          .ToListAsync();
            if (!discounts.Any())
            {
                return Ok(new List<object> { new { name = "Girilmemiş", id = 0 } });
            }
            return Ok(discounts);
        }

    }
}
