using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IUserCartRepository
    {
        IQueryable<UserCart> AllUserCarts { get; }
        void ClearCart(string userId);
        void CreateNewEmptyCart(string userId);
        void AddToCart(int productId, string userId, int quantity);
        void RemoveFromCart(int productId, string userId, int quantity);
    }
}
