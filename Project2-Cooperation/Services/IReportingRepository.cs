using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IReportingRepository
    {
        decimal TotalRevenue(DateTime from, DateTime to);
        decimal NetProfit(DateTime from, DateTime to);
        decimal ProductRevenueToTotalRevenue(Product product, DateTime from, DateTime to);
        decimal CategorySales(string category);
    }
}
