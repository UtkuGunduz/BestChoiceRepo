using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BestChoice.UI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BestChoice.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;

        public OrderController(ILogger<OrderController> logger, IConfiguration configuration = null)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Orders()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Details(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.OrderId = id;
            return View();
        }

        public IActionResult Create()
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
