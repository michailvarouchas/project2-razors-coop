using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    public class WishListController : Controller
    {
        private IProductRepository _productRepo;
        private IWishListRepository _wishListRepo;
        private UserManager<ApplicationUser> _userManager;

        public WishListController(IProductRepository productRepo, IWishListRepository wishListRepo, UserManager<ApplicationUser> userManager)
        {
            _productRepo = productRepo;
            _wishListRepo = wishListRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddToWishList(int id, int qty = 1)
        {
            var userId = _userManager.GetUserId(User);

            _wishListRepo.AddToWishList(id, userId, qty);

            var product = _productRepo.Products.SingleOrDefault(p => p.ProductId == id);

            TempData["message"] = $"Product {product.Title} added to your wishlist!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RemoveFromWishList(int id, int qty = 1)
        {
            var userId = _userManager.GetUserId(User);

            _wishListRepo.RemoveFromWishList(id, userId, qty);

            var product = _productRepo.Products.SingleOrDefault(p => p.ProductId == id);

            TempData["message"] = $"Product {product.Title} removed from your wishlist!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userWishlist = _wishListRepo.AllWishLists.SingleOrDefault(u => u.ApplicationUserId == _userManager.GetUserId(User));
            if (userWishlist != null)
            {
                foreach (var item in userWishlist.WishListItems)
                {
                    item.Product = _productRepo.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                }
                return View(userWishlist);
            }
            else
            {
                return View(userWishlist);
            }
            

            
        }
    }
}