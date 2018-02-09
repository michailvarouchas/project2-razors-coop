using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class CartController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IUserCartRepository _userCartRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IProductRepository prodRepository, IUserCartRepository userCartRepo, 
                                UserManager<ApplicationUser> userManager)
        {
            _prodRepo = prodRepository;
            _userCartRepo = userCartRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var userCart = _userCartRepo.AllUserCarts.SingleOrDefault(u => u.ApplicationUserId == userId);

            //fill with products
            foreach (var item in userCart.CartItems)
            {
                item.Product = _prodRepo.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
            }

            return View(userCart);
        }

        public IActionResult AddToCart(int id, int qty = 1)
        {
            var prodToAdd =_prodRepo.Products.SingleOrDefault(p => p.ProductId == id);

            //db cart
            var userId = _userManager.GetUserId(User);

            _userCartRepo.AddToCart(id, userId, qty);

            TempData["message"] = $"Cart successfully updated";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var userId = _userManager.GetUserId(User);

            var prodToRemove = _prodRepo.Products.SingleOrDefault(p => p.ProductId == id);

            _userCartRepo.RemoveFromCart(id, userId, 1);

            TempData["message"] = $"Cart successfully updated";

            return RedirectToAction(nameof(Index));
        }
    }
}