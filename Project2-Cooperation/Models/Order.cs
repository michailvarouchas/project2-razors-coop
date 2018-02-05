using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        //adds foreign key to CartItems Table
        public virtual ICollection<CartItem> CartItems { get; set; }

        [Required]
        public UserDetails UserDetails { get; set; }

        public DateTime Date { get; set; }

        public bool Canceled { get; set; }

        public bool Completed { get; set; }

        public bool Archived { get; set; }

        public decimal Total { get; set; }
    }
}
