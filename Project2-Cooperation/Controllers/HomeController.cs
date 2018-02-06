using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productsRepo;

        public HomeController(IProductRepository repository)
        {
            _productsRepo = repository;
        }

        public IActionResult Index()
        {
            var featured = _productsRepo.Products.Where(p => p.Featured == true).Take(6);
            return View(featured);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
