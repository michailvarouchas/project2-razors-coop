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
    [Route("api/cart")]
    [Produces("application/json")]
    [Authorize(Roles = "SuperAdmin, Member, User")]
    public class CartAPIController : Controller
    {
        private IProductRepository _prodRepo;
        private IUserCartRepository _userCartRepo;
        private UserManager<ApplicationUser> _userManager;

        public CartAPIController(IProductRepository prodRepo, IUserCartRepository userCartRepo, UserManager<ApplicationUser> userManager)
        {
            _prodRepo = prodRepo;
            _userCartRepo = userCartRepo;
            _userManager = userManager;
        }

        //GET -- api/cart
        [HttpGet]
        public UserCart Get()
        {
            var userId = _userManager.GetUserId(User);

            var userCart = _userCartRepo.AllUserCarts.SingleOrDefault(u => u.ApplicationUserId == userId);

            if (userCart != null)
            {
                //fill with products
                foreach (var item in userCart.CartItems)
                {
                    item.Product = _prodRepo.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                }
            }
            else
            {
                _userCartRepo.CreateNewEmptyCart(userId);
            }

            return userCart;
        }

        //POST -- api/cart/5
        [HttpPost]
        public Product Post([FromBody] string prodId)
        {
            if (prodId != null)
            {
                var id = Int32.Parse(prodId);

                var prodToAdd = _prodRepo.Products.SingleOrDefault(p => p.ProductId == id);

                //_cart.AddToCart(prodToAdd);

                //db cart
                var userId = _userManager.GetUserId(User);

                _userCartRepo.AddToCart(id, userId, 1);

                return prodToAdd;
            }
            return null;
        }

        //DELETE -- api/cart/5
        [HttpDelete]
        public Product Delete([FromBody]string prodId)
        {
            if (prodId != null)
            {
                var id = Int32.Parse(prodId);

                //db cart
                var userId = _userManager.GetUserId(User);

                var prodToRemove = _prodRepo.Products.SingleOrDefault(p => p.ProductId == id);

                _userCartRepo.RemoveFromCart(id, userId, 1);

                return prodToRemove;
            }
            return null;
            
        }
    }
}