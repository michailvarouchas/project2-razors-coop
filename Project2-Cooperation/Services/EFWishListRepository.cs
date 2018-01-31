using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public class EFWishListRepository : IWishListRepository
    {
        private ApplicationDbContext _db;

        public EFWishListRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<WishList> AllWishLists => _db.Whishlist.Include(w => w.WishListItems);

        public void AddToWishList(int productId, string userId, int quantity)
        {
            var wishList = _db.Whishlist.Include(w => w.WishListItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            //TODO --> check if productid exists

            UpdateOrAdd(wishList, productId, quantity, userId);

            _db.SaveChanges();
        }

        public void RemoveFromWishList(int productId, string userId, int quantity)
        {
            var wishList = _db.Whishlist.Include(w => w.WishListItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            RemoveItemOrDecreaseQuantity(wishList, productId, quantity);

            _db.SaveChanges();
        }

        //helpers
        private void UpdateOrAdd(WishList wishList, int productId, int quantity, string userId)
        {
            var wishListItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            };

            if (wishList != null)
            {
                AddNewItemOrIncreaseQuantity(wishList, productId, quantity, wishListItem);
            }
            else
            {
                CreateNewWishList(wishList, wishListItem, userId);
            }
        }

        private void CreateNewWishList(WishList wishList, CartItem wishListItem, string userId)
        {
            wishList = new WishList
            {
                WishListItems = new List<CartItem>()
            };
            wishList.WishListItems.Add(wishListItem);
            wishList.Date = DateTime.Now;
            wishList.ApplicationUserId = userId;
            _db.Whishlist.Add(wishList);
        }

        private void AddNewItemOrIncreaseQuantity(WishList wishList, int productId, int quantity, CartItem wishListItem)
        {
            wishList.Date = DateTime.Now;

            bool isNotAlreadyIn = true;

            foreach (var item in wishList.WishListItems)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity += quantity;
                    isNotAlreadyIn &= false;
                }
            }
            if (isNotAlreadyIn)
            {
                wishList.WishListItems.Add(wishListItem);
            }

            _db.Whishlist.Update(wishList);
        }

        private void RemoveItemOrDecreaseQuantity(WishList wishList, int productId, int quantity)
        {
            wishList.Date = DateTime.Now;

            CartItem cartItemToRemove = new CartItem();

            foreach (var item in wishList.WishListItems)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity -= quantity;
                    if (item.Quantity == 0 )
                    {
                        cartItemToRemove = item;
                    }
                    
                }
            }
            if (cartItemToRemove != null)
            {
                _db.CartItem.Remove(cartItemToRemove);
            }

            _db.Whishlist.Update(wishList);
        }
    }
}
