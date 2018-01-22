using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project2_Cooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.SeedData
{
    public static class SeedProducts
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext db = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            UserManager<ApplicationUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<ApplicationUser>>();

            db.Database.Migrate();

            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Title = "Product1", BuyPrice=1.78M, SalePrice = 5.34M, Category = "Category1", IsLive=true ,Stock = 5, Member = await userManager.FindByEmailAsync("member1@afdemp.gr"), ImageUrl= "/images/products/Category1/apricots-2523272_640.jpg" },
                    new Product { Title = "Product2", BuyPrice = 2.38M, SalePrice = 7.14M, Category = "Category1", IsLive=true, Stock = 5, Member = await userManager.FindByEmailAsync("member1@afdemp.gr"), ImageUrl = "/images/products/Category1/milk-2585128_640.jpg" },
                    new Product { Title = "Product3", BuyPrice = 3.45M, SalePrice = 10.35M, Category = "Category1", IsLive = true, Stock = 5, Member = await userManager.FindByEmailAsync("member2@afdemp.gr"), ImageUrl = "/images/products/Category1/orange-1995079_640.jpg" },
                    new Product { Title = "Product4", BuyPrice = 7.78M, SalePrice = 23.34M, Category = "Category2", IsLive = true, Stock = 5, Member = await userManager.FindByEmailAsync("member2@afdemp.gr"), ImageUrl = "/images/products/Category2/blueberries-2270379_640.jpg" },
                    new Product { Title = "Product5", BuyPrice = 9.38M, SalePrice = 28.14M, Category = "Category2", IsLive = true, Stock = 5, Member = await userManager.FindByEmailAsync("member3@afdemp.gr"), ImageUrl = "/images/products/Category2/honey-823614_640.jpg" },
                    new Product { Title = "Product6", BuyPrice = 11.45M, SalePrice = 34.35M, Category = "Category2", IsLive = true, Stock = 5, Member = await userManager.FindByEmailAsync("member3@afdemp.gr"), ImageUrl = "/images/products/Category2/watermelon-815072_640.jpg" }
                    );
                db.SaveChanges();
            }
        }
    }
}
