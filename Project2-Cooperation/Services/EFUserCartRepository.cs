using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public class EFUserCartRepository : IUserCartRepository
    {
        private ApplicationDbContext _db;

        public EFUserCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void ClearCart(string userId)
        {
            var userCart = _db.UserCart.Include(w => w.CartItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            foreach (var item in userCart.CartItems)
            {
                _db.UserCartItems.Remove(item);
            }

            _db.SaveChanges();
        }

        public void CreateNewEmptyCart(string userId)
        {
            var userCart = new UserCart
            {
                Date = DateTime.Now,
                ApplicationUserId = userId,
                CartItems = new List<UserCartItem>()
            };
            _db.UserCart.Add(userCart);

            _db.SaveChanges();
        }

        public IQueryable<UserCart> AllUserCarts => _db.UserCart.Include(w => w.CartItems);

        public void AddToCart(int productId, string userId, int quantity)
        {
            var userCart = _db.UserCart.Include(w => w.CartItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            //TODO --> check if productid exists

            UpdateOrAdd(userCart, productId, quantity, userId);

            _db.SaveChanges();
        }

        public void RemoveFromCart(int productId, string userId, int quantity)
        {
            var wishList = _db.UserCart.Include(w => w.CartItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            RemoveItemOrDecreaseQuantity(wishList, productId, quantity);

            _db.SaveChanges();
        }

        //helpers
        private void UpdateOrAdd(UserCart userCart, int productId, int quantity, string userId)
        {
            var cartItem = new UserCartItem
            {
                ProductId = productId,
                Quantity = quantity
            };

            if (userCart != null)
            {
                AddNewItemOrIncreaseQuantity(userCart, productId, quantity, cartItem);
            }
            else
            {
                CreateNewCart(userCart, cartItem, userId);
            }
        }

        private void CreateNewCart(UserCart userCart, UserCartItem cartItem, string userId)
        {
            userCart = new UserCart
            {
                CartItems = new List<UserCartItem>()
            };
            userCart.CartItems.Add(cartItem);
            userCart.Date = DateTime.Now;
            userCart.ApplicationUserId = userId;
            _db.UserCart.Add(userCart);
        }

        private void AddNewItemOrIncreaseQuantity(UserCart userCart, int productId, int quantity, UserCartItem cartItem)
        {
            userCart.Date = DateTime.Now;

            bool isNotAlreadyIn = true;

            foreach (var item in userCart.CartItems)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity += quantity;
                    isNotAlreadyIn &= false;
                }
            }
            if (isNotAlreadyIn)
            {
                userCart.CartItems.Add(cartItem);
            }

            _db.UserCart.Update(userCart);
        }

        private void RemoveItemOrDecreaseQuantity(UserCart userCart, int productId, int quantity)
        {
            userCart.Date = DateTime.Now;

            foreach (var item in userCart.CartItems)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity -= quantity;
                    if (item.Quantity == 0 )
                    {
                        _db.UserCartItems.Remove(item);
                    }
                }
            }

            _db.UserCart.Update(userCart);
        }
    }
}
