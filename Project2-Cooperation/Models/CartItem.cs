using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Order Order { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotals => Product.SalePrice * Quantity;

        public CartItem()
        {

        }

        public CartItem(Product product, int quantity)
        {
            ProductId = product.ProductId;
            Product = product;
            Quantity = quantity;
        }    
    }
}
