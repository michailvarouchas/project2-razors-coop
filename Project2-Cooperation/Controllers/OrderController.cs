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
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IOrderRepository _ordersRepo;
        private IProductRepository _productsRepo;
        private IUserDetailsRepository _userDetailsRepo;
        private Cart _cart;

        public OrderController(UserManager<ApplicationUser> userManager, IOrderRepository ordersRepo, IProductRepository productsRepo, IUserDetailsRepository userDetailsRepo, Cart cartService)
        {
            _userDetailsRepo = userDetailsRepo;
            _userManager = userManager;
            _ordersRepo = ordersRepo;
            _productsRepo = productsRepo;
            _cart = cartService;
        }

        public IActionResult CheckOut()
        {
            var loggedUserId = _userManager.GetUserId(User);

            var checkOut = new CheckOutViewModel
            {
                Cart = _cart,
                Order = new Order()
                {
                    UserDetails = _userDetailsRepo.UserDetails.SingleOrDefault(u => u.ApplicationUserId == loggedUserId)
                }
            };

            return View(checkOut);
        }

        [HttpPost]
        public IActionResult CheckOut(CheckOutViewModel checkOut)
        {
            if (ModelState.IsValid)
            {
                bool availability = true;

                //get cartItems from Session and add them into the order
                checkOut.Order.CartItems = _cart.CartItems;

                checkOut.Order.Date = DateTime.Now;

                checkOut.Order.Total = _ordersRepo.GetOrderTotal(checkOut.Order);

                //add applicationUserId
                checkOut.Order.UserDetails.ApplicationUserId = _userManager.GetUserId(User);

                //check availability
                foreach (var cartItem in checkOut.Order.CartItems)
                {
                    if (!IsAvailable(cartItem))
                    {
                        _cart.RemoveFromCart(cartItem.Product, cartItem.Quantity);
                        availability &= false;
                    }
                }
                if (!availability)
                {
                    checkOut.Cart = _cart;
                    ViewData["failMessage"] = "Some products are no longer available thus removed from your cart!";
                    return View(checkOut);
                }
                
                //add transaction

                foreach (var cartItem in checkOut.Order.CartItems)
                {
                    ReduceProductStock(cartItem);
                }
                    
                _ordersRepo.CreateOrder(checkOut.Order);

                return RedirectToAction(nameof(Completed));
            }

            checkOut.Cart = _cart;
            return View(checkOut);

        }

        public ActionResult Completed()
        {
            _cart.ClearCart();

            return View();
        }

        private void ReduceProductStock(CartItem ci)
        {
            var productToUpdate = _productsRepo.Products.FirstOrDefault(p => p.ProductId == ci.ProductId);

            productToUpdate.Stock -= ci.Quantity;

            _productsRepo.UpdateProduct(productToUpdate);
        }

        private bool IsAvailable(CartItem ci)
        {
            var productToConfirm = _productsRepo.Products.FirstOrDefault(p => p.ProductId == ci.ProductId);

            return productToConfirm.Stock >= ci.Quantity ? true : false;
        }
    }
}