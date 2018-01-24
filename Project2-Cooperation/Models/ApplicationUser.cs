using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Project2_Cooperation.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual UserDetails UserDetails { get; set; }

        public virtual InternalAccount InternalAccount { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }

        public virtual WishList Wishlist { get; set; }

        public ApplicationUser() : base()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }
    }
}
