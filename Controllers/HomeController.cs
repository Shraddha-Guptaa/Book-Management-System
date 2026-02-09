using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyWeb.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofwork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objcategoires = _unitofwork.Product.GetAll(includeproperties: "category").ToList();//calling data from table
            return View(objcategoires);
        }

        public IActionResult Details(int? id)
        {
            Product product = _unitofwork.Product.Get(u => u.ProductId == id, includeproperties:"category");

            return View(product);
        }   
        [Authorize(Roles = "Admin")]

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //[Authorize(Roles = "user")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return View("Login");
        }
    }
}
