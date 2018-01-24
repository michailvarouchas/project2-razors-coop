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
    [Route("api/wishlist")]
    [Produces("application/json")]
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

        [HttpPost]
        public Product Post([FromBody] string prodId, int qty = 1)
        {
            if (prodId != null)
            {
                bool res = int.TryParse(prodId, out int productId);

                var userId = _userManager.GetUserId(User);

                _wishListRepo.AddToWishList(productId, userId, qty);

                var product = _productRepo.Products.SingleOrDefault(p => p.ProductId == productId);

                return product;
            }

            return null;
        }
    }
}