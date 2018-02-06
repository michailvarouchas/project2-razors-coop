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
using Microsoft.EntityFrameworkCore;

namespace Project2_Cooperation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IOrderRepository _ordersRepo;
        private IProductRepository _productsRepo;
        private IUserCartRepository _userCartRepo;
        private IUserDetailsRepository _userDetailsRepo;
        private ITransactionRepository _transactionRepository;

        public OrderController(UserManager<ApplicationUser> userManager, IOrderRepository ordersRepo,
            IProductRepository productsRepo, IUserDetailsRepository userDetailsRepo,
            ITransactionRepository transactionRepository, IUserCartRepository userCartRepo)
        {
            _userDetailsRepo = userDetailsRepo;
            _userManager = userManager;
            _ordersRepo = ordersRepo;
            _userCartRepo = userCartRepo;
            _productsRepo = productsRepo;
            _transactionRepository = transactionRepository;
        }

        public IActionResult CheckOut()
        {
            var loggedUserId = _userManager.GetUserId(User);

            var checkOut = new CheckOutViewModel
            {
                Cart = _userCartRepo.AllUserCarts.SingleOrDefault(u => u.ApplicationUserId == loggedUserId),
                Order = new Order()
                {
                    UserDetails = _userDetailsRepo.UserDetails.SingleOrDefault(u => u.ApplicationUserId == loggedUserId)
                }
            };

            foreach (var item in checkOut.Cart.CartItems)
            {
                item.Product = _productsRepo.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
            }

            return View(checkOut);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutViewModel checkOut)
        {
            var userId = _userManager.GetUserId(User);
            var dbCart = _userCartRepo.AllUserCarts.SingleOrDefault(u => u.ApplicationUserId == userId);

            if (ModelState.IsValid)
            {
                bool availability = true;

                //var cartItems = new List<CartItem>();

                //foreach (var item in dbCart.CartItems)
                //{
                //    var ci = new CartItem
                //    {
                //        ProductId = item.ProductId,
                //        Quantity = item.Quantity
                //    };
                //    cartItems.Add(ci);
                //}

                //get cartItems from Session and add them into the order
                var ciList = new List<CartItem>();

                foreach (var item in dbCart.CartItems)
                {
                    var ci = new CartItem
                    {
                        ProductId = item.ProductId,
                        Product = _productsRepo.Products.SingleOrDefault(p => p.ProductId == item.ProductId),
                        Quantity = item.Quantity,

                    };
                    ciList.Add(ci);
                }
                checkOut.Order.CartItems = ciList;

                checkOut.Order.Date = DateTime.Now;

                checkOut.Order.Total = _ordersRepo.GetOrderTotal(checkOut.Order);

                //add applicationUserId
                checkOut.Order.UserDetails.ApplicationUserId = userId;

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
                    if (stock < cartItem.Quantity)
                    {
                        _userCartRepo.RemoveFromCart(cartItem.ProductId, userId, cartItem.Quantity - stock);
                        //_cart.RemoveFromCart(cartItem.Product, cartItem.Quantity - stock);
                        availability &= false;
                    }
                }

                if (!availability)
                {
                    foreach (var item in ctr)
                    {
                        _userCartRepo.RemoveFromCart(item.ProductId, userId, item.Quantity);
                        //_cart.RemoveFromCart(item.Product, item.Quantity);
                    }

                    TempData["failMessage"] = "Some products are no longer available thus removed from your cart!";
                    checkOut.Cart = dbCart;
                    return View(checkOut);
                }

                if (checkOut.Order.CartItems.Count() != 0)
                {

                    //add transaction
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
                        TempData["failMessage"] = $"Insufficient balance. Your current balance is {_transactionRepository.UserBalance(userId).ToString("C2")}.";
                        checkOut.Cart = dbCart;
                        return View(checkOut);
                    }

                }

                TempData["failMessage"] = "Products in your cart are no longer available!";
            }

            checkOut.Cart = dbCart;
            return View(checkOut);

        }

        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var orderToCancel = _ordersRepo.GetOrders().Include(c => c.CartItems).SingleOrDefault(o => o.OrderId == id);

            //roll-back product stock
            foreach (var cartItem in orderToCancel.CartItems)
            {
                RollBackProductStock(cartItem);
            }

            //return money
            var userId = orderToCancel.UserDetails.ApplicationUserId;
            var admin = await _userManager.FindByEmailAsync("admin@afdemp.gr");
            var adminId = admin.Id;

            var members = await GetMembers();

            _transactionRepository.ReturnMoney(adminId, userId, members, orderToCancel.Total);
            _ordersRepo.CanceledOrder(orderToCancel);
            return RedirectToAction("ViewOrders", "Admin");

        }

        public ActionResult Completed()
        {
            var userId = _userManager.GetUserId(User);

            _userCartRepo.ClearCart(userId);

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

        private void RollBackProductStock(CartItem ci)
        {
            var productToUpdate = _productsRepo.Products.FirstOrDefault(p => p.ProductId == ci.ProductId);

            productToUpdate.Stock += ci.Quantity;

            _productsRepo.UpdateProduct(productToUpdate);
        }
    }

}