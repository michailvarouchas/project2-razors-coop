using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.EshopViewModels
{
    public class ViewOrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public string Phone { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
