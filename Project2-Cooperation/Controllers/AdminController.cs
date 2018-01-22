using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Models;
using Project2_Cooperation.Models.EshopViewModels;
using Project2_Cooperation.Services;

namespace Project2_Cooperation.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productsRepo;
        private readonly IOrderRepository _ordersRepo;

        public AdminController(UserManager<ApplicationUser> userManager, IProductRepository productsRepo, 
            IOrderRepository ordersRepo)
        {
            _userManager = userManager;
            _productsRepo = productsRepo;
            _ordersRepo = ordersRepo;
        }

        public IActionResult Index()
        {
            return View(_productsRepo.Products.Include(p => p.Member));
        }

        public IActionResult ViewOrders()
        {
            var orders = _ordersRepo.GetOrders();

            //fill in the Products
            foreach (var order in orders)
            {
                foreach (var item in order.CartItems)
                {
                    item.Product = _ordersRepo.GetCartItemProduct(item);
                }
            }

            return View(new ViewOrdersViewModel() { Orders = orders });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilterByDate(ViewOrdersViewModel vm)
        {
            var orders = _ordersRepo.GetOrders().Where(o => o.Date >= vm.DateStart && o.Date <= vm.DateEnd.AddDays(1));

            //fill in the Products
            foreach (var order in orders)
            {
                foreach (var item in order.CartItems)
                {
                    item.Product = _ordersRepo.GetCartItemProduct(item);
                }
            }
            ViewData["filter"] = $"Orders from \"{vm.DateStart.ToString("dd/MM/yyyy")}\" to \"{vm.DateEnd.ToString("dd/MM/yyyy")}\"";
            return View(nameof(ViewOrders), new ViewOrdersViewModel() { Orders = orders , DateStart = vm.DateStart, DateEnd = vm.DateEnd, Phone = vm.Phone});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilterByPhone(ViewOrdersViewModel vm)
        {
            var orders = _ordersRepo.GetOrders().Where(o => o.UserDetails.Phone == vm.Phone);

            //fill in the Products
            foreach (var order in orders)
            {
                foreach (var item in order.CartItems)
                {
                    item.Product = _ordersRepo.GetCartItemProduct(item);
                }
            }
            ViewData["filter"] = $"Orders with phone: \"{vm.Phone}\"";
            return View(nameof(ViewOrders) ,new ViewOrdersViewModel() { Orders = orders, Phone = vm.Phone });
        }

        public IActionResult DeleteOrder(int id)
        {
            _ordersRepo.DeleteOrder(id);

            TempData["message"] = $"Order {id} deleted successfully.";

            return RedirectToAction(nameof(ViewOrders));
        }

        public IActionResult MarkAsCompleted(int id)
        {
            Order orderToUpdate = _ordersRepo.GetOrders().SingleOrDefault(o => o.OrderId == id);

            orderToUpdate.Completed = true;

            _ordersRepo.UpdateOrder(orderToUpdate);

            TempData["message"] = $"Order {id} updated successfully.";

            return RedirectToAction(nameof(ViewOrders));
        }

        public IActionResult MarkAsPending(int id)
        {
            Order orderToUpdate = _ordersRepo.GetOrders().SingleOrDefault(o => o.OrderId == id);

            orderToUpdate.Completed = false;

            _ordersRepo.UpdateOrder(orderToUpdate);

            TempData["message"] = $"Order {id} updated successfully.";

            return RedirectToAction(nameof(ViewOrders));
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Product product = _productsRepo.Products.Include(m => m.Member).SingleOrDefault(p => p.ProductId == id);

            product.MemberId = currentUser.Id;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await Product.CopyImageToServer(product);

                _productsRepo.UpdateProduct(product);

                //update transactions table pay member if the product is accepted (add price) 

                TempData["message"] = $"Product {product.Title} updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = _productsRepo.Products.SingleOrDefault(p => p.ProductId == id);

            _productsRepo.DeleteProduct(id);

            TempData["message"] = $"Product {productToDelete.Title} deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewUsers()
        {
            List<ViewUsersViewModel> vm = new List<ViewUsersViewModel>();
            var users = _userManager.Users.Include(d => d.UserDetails);

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                vm.Add(new ViewUsersViewModel { User = user.Email, Roles = roles.ToList()});
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == id);

            var roles = await _userManager.GetRolesAsync(user);

            return View(new EditUserViewModel { User = user.Email, Roles = roles.ToList() });
        }

        [HttpPost]
        public async Task<IActionResult> EditUser (EditUserViewModel vm)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == vm.User);

            foreach (var role in vm.AllRoles)
            {
                bool isInRole = await _userManager.IsInRoleAsync(user, role.Value);

                if (isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Value);
                }
            }
            
            var addedRoles = await _userManager.AddToRolesAsync(user, vm.Roles);

            return RedirectToAction(nameof(ViewUsers));
        }
    }
}