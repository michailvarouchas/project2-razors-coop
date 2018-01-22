using Microsoft.AspNetCore.Mvc;
using Project2_Cooperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Components
{
    public class NavCartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public NavCartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
