using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BestChoice.UI.Models;

namespace BestChoice.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration = null)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult About()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Contact()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Privacy()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Terms()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }
        public IActionResult Blog()
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
