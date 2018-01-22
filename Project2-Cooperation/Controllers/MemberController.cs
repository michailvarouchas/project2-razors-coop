using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    public class MemberController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(UserManager<ApplicationUser> userManager, IProductRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return View(_repository.Products.Where(p => p.Member == currentUser));
        }

        public async Task<IActionResult> CreateProduct()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var product = new Product
            {
                MemberId = currentUser.Id
            };
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await Product.CopyImageToServer(product);

                _repository.CreateProduct(product);

                TempData["message"] = "Product Created Successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(product);

        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Product product = _repository.Products.Include(m => m.Member).SingleOrDefault(p => p.ProductId == id);

            product.MemberId = currentUser.Id;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await Product.CopyImageToServer(product);

                _repository.UpdateProduct(product);

                //update transactions table pay member if the product is accepted (add price) 

                TempData["message"] = $"Product {product.Title} updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}