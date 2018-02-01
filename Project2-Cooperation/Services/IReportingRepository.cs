using Project2_Cooperation.Models;
using Project2_Cooperation.Models.ReportingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Services
{
    public interface IReportingRepository
    {
        decimal LastYearSales();
        List<MonthSales> LastYearSalesByMonth();
        IQueryable<ProductSales> TopSellingProducts(int numberOfProducts);
        IQueryable<CategorySales> SalesByCategory(int numberOfCategories);
    }
}
