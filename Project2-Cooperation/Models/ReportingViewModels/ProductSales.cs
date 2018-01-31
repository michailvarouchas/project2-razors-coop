using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.ReportingViewModels
{
    public class ProductSales
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Sales { get; set; }
    }
}
