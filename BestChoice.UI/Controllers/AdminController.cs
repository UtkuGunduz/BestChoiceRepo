using BestChoice.UI.Controllers;
using BestChoice.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BestChoice.UI.Models;

namespace BestChoice.UI.Controllers
{

    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AdminController(ILogger<AccountController> logger, IConfiguration configuration = null)
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

        public IActionResult Blank()
        {
            return View();
        }

        public IActionResult ListUsers()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult RemoveUsers(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.UserId = id;
            return View();
        }
        public IActionResult UpdateUsers(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.UserId = id;
            return View();
        }

        public IActionResult GetUser(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.UserId = id;
            return View();
        }


        public IActionResult ListRoles()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult AddRoles()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult EditRoles()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }
        public IActionResult RemoveRoles()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }


        public IActionResult ListProducts()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult AddProducts()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult EditProducts()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult RemoveProducts()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult ListCategories()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult AddCategories()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult EditCategories(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.CategoryId = id;
            return View();
        }

        public IActionResult RemoveCategories(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.CategoryId = id;
            return View();
        }


        public IActionResult ListOrders()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult EditOrders(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.CategoryId = id;
            return View();
        }

        public IActionResult RemoveOrders(int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.CategoryId = id;
            return View();
        }

    }
}

