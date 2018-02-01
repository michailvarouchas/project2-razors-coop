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
using System.Collections;

namespace Project2_Cooperation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IOrderRepository _ordersRepo;
        private IProductRepository _productsRepo;
        private IUserDetailsRepository _userDetailsRepo;
        private ITransactionRepository _transactionRepository;
        private Cart _cart;

        public OrderController(UserManager<ApplicationUser> userManager, IOrderRepository ordersRepo,
            IProductRepository productsRepo, IUserDetailsRepository userDetailsRepo, ITransactionRepository transactionRepository, Cart cartService)
        {
            _userDetailsRepo = userDetailsRepo;
            _userManager = userManager;
            _ordersRepo = ordersRepo;
            _productsRepo = productsRepo;
            _transactionRepository = transactionRepository;
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
        public async Task <IActionResult> CheckOut(CheckOutViewModel checkOut)
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
                var ctr = new List<CartItem>();

                foreach (var cartItem in checkOut.Order.CartItems)
                {
                    var stock = GetAvailableStock(cartItem);
                    if (stock == 0)
                    {
                        ctr.Add(cartItem);
                        availability &= false;
                    }
                    if(stock < cartItem.Quantity)
                    {
                        _cart.RemoveFromCart(cartItem.Product, cartItem.Quantity - stock);
                        availability &= false;
                    }
                }
                foreach (var item in ctr)
                {
                    _cart.RemoveFromCart(item.Product, item.Quantity);
                }
                if (!availability)
                {
                    checkOut.Cart = _cart;
                    TempData["failMessage"] = "Some products are no longer available thus removed from your cart!";
                    return View(checkOut);
                }

                

                if (checkOut.Order.CartItems.Count() != 0)
                {

                    //add transaction
                    var userId = _userManager.GetUserId(User);
                    var admin = await _userManager.FindByEmailAsync("admin@afdemp.gr");
                    var adminId = admin.Id;

                    var members = await GetMembers();

                    var success = _transactionRepository.TransactionCheckout(adminId, userId, members, checkOut.Order.Total);
                    if (success)
                    {
                        //reduce product stock
                        foreach (var cartItem in checkOut.Order.CartItems)
                        {
                            ReduceProductStock(cartItem);
                        }
                        _ordersRepo.CreateOrder(checkOut.Order);
                        return RedirectToAction(nameof(Completed));
                    }
                    else
                    {
                        TempData["failMessage"] = $"Insufficient balance. Your current balance is {_transactionRepository.UserBalance(userId)}.";

                        return RedirectToAction(nameof(checkOut));
                    }


                }

                TempData["failMessage"] = "Products in your cart are no longer available!";
            }

            checkOut.Cart = _cart;
            return View(checkOut);

        }

        public ActionResult Completed()
        {
            _cart.ClearCart();

            return View();
        }

        private async Task<IEnumerable<ApplicationUser>> GetMembers()
        {
            var members = await _userManager.GetUsersInRoleAsync("Member");
            return members;
        }

        private void ReduceProductStock(CartItem ci)
        {
            var productToUpdate = _productsRepo.Products.FirstOrDefault(p => p.ProductId == ci.ProductId);

            productToUpdate.Stock -= ci.Quantity;

            _productsRepo.UpdateProduct(productToUpdate);
        }

        private int GetAvailableStock(CartItem ci)
        {
            var productToConfirm = _productsRepo.Products.FirstOrDefault(p => p.ProductId == ci.ProductId);

            return productToConfirm.Stock >= ci.Quantity ? ci.Quantity : productToConfirm.Stock;
        }
    }
}