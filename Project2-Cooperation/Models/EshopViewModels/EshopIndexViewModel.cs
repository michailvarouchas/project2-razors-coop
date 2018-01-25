using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.EshopViewModels
{
    public class EshopIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string Category { get; set; }
    }
}
