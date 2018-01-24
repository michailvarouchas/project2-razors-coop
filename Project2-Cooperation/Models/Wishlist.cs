using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class WishList
    {
        [Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set;}
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime Date { get; set; }

        public ICollection<CartItem> WishListItems { get; set; }
    }
}
