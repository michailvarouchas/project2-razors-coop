using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.ReportingViewModels
{
    public class SalesViewModel
    {
        public IQueryable<ProductSales> SalesByProduct { get; set; }
        public IQueryable<CategorySales> SalesByCategory { get; set; }
        public List<decimal> YearSalesByMonth { get; set; }
        public decimal YearSales { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
