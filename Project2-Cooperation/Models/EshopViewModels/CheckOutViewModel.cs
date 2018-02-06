using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.EshopViewModels
{
    public class CheckOutViewModel
    {
        public Order Order { get; set; }
        public UserCart Cart { get; set; }
    }
}
