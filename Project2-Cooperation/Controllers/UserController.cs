using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Models.EshopViewModels;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    [Authorize(Roles = "SuperAdmin ,Member, User")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productsRepo;
        private readonly IOrderRepository _ordersRepo;

        public UserController(UserManager<ApplicationUser> userManager, IProductRepository repository, IOrderRepository ordersRepo)
        {
            _userManager = userManager;
            _productsRepo = repository;
            _ordersRepo = ordersRepo;
        }

        public IActionResult Index(string category)
        {
            if (category != null)
            {
                var products = _productsRepo.Products.Where(p => p.BoughtFromAdmin == true && p.IsLive == true && p.Category == category);

                return View(new EshopIndexViewModel { Products=products, Category = category});
            }
            else
            {
                var products = _productsRepo.Products.Where(p => p.BoughtFromAdmin == true && p.IsLive == true);

                return View(new EshopIndexViewModel { Products = products, Category = null });
            }
            
        }

        public IActionResult SingleProduct(int id)
        {
            var product = _productsRepo.Products.SingleOrDefault(p => p.ProductId == id);

            return View(new CartItem { Product = product, Quantity = 1 });
        }

        public IActionResult ViewOrders()
        {
            var orders = _ordersRepo.GetOrders().Where(o =>
                        o.UserDetails.ApplicationUserId == _userManager.GetUserId(User));

            //fill in the Products
            foreach (var order in orders)
            {
                foreach (var item in order.CartItems)
                {
                    item.Product = _ordersRepo.GetCartItemProduct(item);
                }
            }

            return View(orders);
        }

    }
}