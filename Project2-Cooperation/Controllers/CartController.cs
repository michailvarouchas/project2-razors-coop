using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;
        private Cart _cart;

        public CartController(IProductRepository prodRepository, Cart cartService)
        {
            _repository = prodRepository;
            _cart = cartService;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        public IActionResult AddToCart(int id, int qty = 1)
        {
            var prodToAdd =_repository.Products.SingleOrDefault(p => p.ProductId == id);

            _cart.AddToCart(prodToAdd);
            
            TempData["message"] = $"Cart successfully updated";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var prodToRemove = _cart.CartItems.SingleOrDefault(ci => ci.Product.ProductId == id).Product;

            _cart.RemoveFromCart(prodToRemove);

            TempData["message"] = $"Cart successfully updated";

            return RedirectToAction(nameof(Index));
        }
    }
}