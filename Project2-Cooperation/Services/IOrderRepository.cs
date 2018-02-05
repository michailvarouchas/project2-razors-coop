using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrders();
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int Id);
        Product GetCartItemProduct(CartItem ci);
        decimal GetOrderTotal(Order order);
        void ArchiveOrder(Order order);
        void CanceledOrder(Order order);
    }
}
