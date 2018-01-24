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

        public IQueryable<WishList> WishLists => _db.Whishlist;

        public void AddToWishList(int productId, string userId, int quantity)
        {
            var wishList = _db.Whishlist.Include(w => w.WishListItems).SingleOrDefault(w => w.ApplicationUserId == userId);

            //create wishList item from productId and quantity
            var wishListItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            };

            if (wishList != null)
            {
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
            else
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
            _db.SaveChanges();
        }

        public void RemoveFromWishList(int productId, string userId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
