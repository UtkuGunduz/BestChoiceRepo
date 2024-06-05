using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BestChoice.UI.Models;

namespace BestChoice.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;

        public ProductController(ILogger<ProductController> logger, IConfiguration configuration = null)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Shop()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Details(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.ProductId = id;
            return View();
        }

        public IActionResult Compare()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Filter()
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
