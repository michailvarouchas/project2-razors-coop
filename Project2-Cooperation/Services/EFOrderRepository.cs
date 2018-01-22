using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models;

namespace Project2_Cooperation.Services
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext _db;

        public EFOrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Order> GetOrders()
        {
            var orders = _db.Orders.Include(f => f.CartItems).Include(c => c.UserDetails);
            return orders;
        }

        public void CreateOrder(Order order)
        {
            //prevent product re-saving
            foreach (var item in order.CartItems)
            {
                item.Product = null;
            }

            if (_db.UserDetails.SingleOrDefault(d => d.ApplicationUserId == order.UserDetails.ApplicationUserId) != null)
            {
                _db.Orders.Update(order);
            }
            else
            {
                _db.Orders.Add(order);
            }
            _db.SaveChanges();
        }

        public void DeleteOrder(int Id)
        {
            Order orderToDelete = _db.Orders.Include(ci => ci.CartItems).FirstOrDefault(p=> p.OrderId == Id);

            if (orderToDelete != null)
            {
                foreach (var item in orderToDelete.CartItems)
                {
                    _db.Remove(item);
                }

                _db.Orders.Remove(orderToDelete);
                _db.SaveChanges();
            }            
        }

        public void UpdateOrder(Order order)
        {
            _db.Orders.Update(order);
            _db.SaveChanges();
        }

        //SOME EXTRA METHODS to help retrieve orders from db
        public Product GetCartItemProduct(CartItem ci)
        {
            return _db.Products.SingleOrDefault(p => p.ProductId == ci.ProductId);
        }

        //USE ONLY IF order.CartItems...Product != null
        public decimal GetOrderTotal(Order order)
        {
            return order.CartItems.Sum(ci => ci.Quantity * ci.Product.SalePrice);
        }
    }
}
