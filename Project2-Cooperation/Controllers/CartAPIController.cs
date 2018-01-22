using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    [Route("api/cart")]
    [Produces("application/json")]
    public class CartAPIController : Controller
    {
        private IProductRepository _repository;
        private Cart _cart;

        public CartAPIController(IProductRepository prodRepository, Cart cartService)
        {
            _repository = prodRepository;
            _cart = cartService;
        }

        //GET -- api/cart
        [HttpGet]
        public Cart Get()
        {
            return _cart;
        }

        //POST -- api/cart/5
        [HttpPost]
        public Product Post([FromBody] string prodId)
        {
            var id = Int32.Parse(prodId);

            var prodToAdd = _repository.Products.SingleOrDefault(p => p.ProductId == id);

            _cart.AddToCart(prodToAdd);

            return prodToAdd;
        }

        //DELETE -- api/cart/5
        [HttpDelete]
        public Product Delete([FromBody]string productId)
        {
            var id = Int32.Parse(productId);

            var cartItemToRemove = _cart.CartItems.SingleOrDefault(ci => ci.Product.ProductId == id);

            var prodToRemove = cartItemToRemove.Product;

            var quantity = cartItemToRemove.Quantity;

            _cart.RemoveFromCart(prodToRemove, quantity);

            return prodToRemove;
        }
    }
}