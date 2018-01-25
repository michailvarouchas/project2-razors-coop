using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IWishListRepository
    {
        IQueryable<WishList> AllWishLists { get; }
        void AddToWishList(int productId, string userId, int quantity);
        void RemoveFromWishList(int productId, string userId, int quantity);
    }
}
