using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BestChoice.UI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BestChoice.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IConfiguration _configuration;

        public CartController(ILogger<CartController> logger, IConfiguration configuration = null)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Cart()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Checkout()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
