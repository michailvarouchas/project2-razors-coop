using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Data;
using Project2_Cooperation.Models.ReportingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public class EFReportingRepository : IReportingRepository
    {
        private ApplicationDbContext _db;
        private IOrderRepository _orderRepository;

        public EFReportingRepository(ApplicationDbContext db, IOrderRepository orderRepository)
        {
            _db = db;
            _orderRepository = orderRepository;
        }

        public decimal LastYearSales()
        {
            var res = _orderRepository.GetOrders().Where(o => o.Date <= BeginingOfCurrentMonth.AddMonths(1) && o.Date >= BeginingOfCurrentMonth.AddMonths(-10) && o.Completed).Sum(o => o.Total);
            return res;
        }

        public List<MonthSales> LastYearSalesByMonth()
        {
            //list that holds last year sales by month
            var lastYearSalesByMonth = new List<MonthSales>();

            for (int i = 0; i < 11; i++)
            {
                //get all orders inside one month
                var mOrders = _orderRepository.GetOrders()
                    .Where(
                        o =>
                        o.Date <= BeginingOfCurrentMonth.AddMonths(1 - i) &&
                        o.Date >= BeginingOfCurrentMonth.AddMonths(-i) &&
                        o.Completed
                );

                //sum all order totals within that month
                var msales = new MonthSales
                {
                    Sales = mOrders.Sum(o => o.Total),
                    Month = BeginingOfCurrentMonth.AddMonths(-i)
                };

                lastYearSalesByMonth.Add(msales);
            }
            return lastYearSalesByMonth;
        }

        public IQueryable<ProductSales> TopSellingProducts(int numberOfProducts)
        {
            var pSales = from o in _db.Orders
                         where o.Date <= BeginingOfCurrentMonth.AddMonths(1) && o.Date >= BeginingOfCurrentMonth.AddMonths(-10)
                         join ci in _db.CartItems on o.OrderId equals ci.Order.OrderId into orderedItems
                         from oi in orderedItems
                         join prod in _db.Products on oi.ProductId equals prod.ProductId
                         group oi by new { prod.ProductId, prod.Title } into pGroup
                         orderby pGroup.Sum(p => p.Quantity * p.Product.SalePrice) descending
                         select new ProductSales()
                         {
                             ProductTitle = pGroup.Key.Title,
                             Quantity = pGroup.Sum(q => q.Quantity),
                             Sales = pGroup.Sum(p => p.Quantity * p.Product.SalePrice)
                         };

            return pSales.Take(numberOfProducts);
        }

        public IQueryable<CategorySales> SalesByCategory(int numberOfCategories)
        {
            var cSales = from o in _db.Orders
                         where o.Date <= BeginingOfCurrentMonth.AddMonths(1) && o.Date >= BeginingOfCurrentMonth.AddMonths(-10)
                         join ci in _db.CartItems on o.OrderId equals ci.Order.OrderId into orderedItems
                         from oi in orderedItems
                         join prod in _db.Products on oi.ProductId equals prod.ProductId
                         group oi by new { prod.Category } into pGroup
                         orderby pGroup.Sum(p => p.Quantity * p.Product.SalePrice) descending
                         select new CategorySales()
                         {
                             Category = pGroup.Key.Category,
                             Total = pGroup.Sum(p => p.Quantity * p.Product.SalePrice)
                         };

            return cSales;
        }

        //helpers
        private DateTime BeginingOfCurrentMonth
        {
            get
            {
                int clearMonth = -DateTime.Now.Day + 1;

                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;

                DateTime date = new DateTime(year, month, 1);

                return date;
            }
        }
    }
}
