using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models.EshopViewModels;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    public class SalesController : Controller
    {
        private readonly IOrderRepository _orderRepo;

        public SalesController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            var lastMonthOrders = _orderRepo.GetOrders().Where(o => o.Date <= DateTime.Now && o.Date >= DateTime.Now.AddMonths(-1) && o.Completed);

            decimal lastMonthSales = 0;

            foreach (var item in lastMonthOrders)
            {
                lastMonthSales += item.Total;
            }

            ViewData["currenttab"] = "sales";

            return View(lastMonthSales);
        }
    }
}