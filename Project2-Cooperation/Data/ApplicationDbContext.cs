﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2_Cooperation.Models;

namespace Project2_Cooperation.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<WishList> Wishlist { get; set; }
        public DbSet<UserCart> UserCart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserCartItem> UserCartItems { get; set; }
        public DbSet<InternalAccount> InternalAccounts { get; set; }
    }
}
