using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class Cart
    {
        private List<CartItem> _cartItems = new List<CartItem>();

        public List<CartItem> CartItems => _cartItems;

        [DataType(DataType.Currency)]
        public virtual decimal CalculateCartTotalCost => _cartItems.Sum(i => i.Subtotals);

        public virtual void AddToCart(Product product, int quantity = 1)
        {
            var cartItemToEditQuantity = _cartItems.FirstOrDefault(c => c.Product.ProductId == product.ProductId);

            if (cartItemToEditQuantity == null)
            {
                _cartItems.Add(new CartItem(product, quantity));
            }
            else
            {
                cartItemToEditQuantity.Quantity += quantity;
            }

        }

        public virtual void RemoveFromCart(Product product, int quantity = 1 )
        {
            var cartItemToRemove = _cartItems.FirstOrDefault(c => c.Product.ProductId == product.ProductId);

            if (cartItemToRemove != null)
            {
                cartItemToRemove.Quantity -= quantity;

                if (cartItemToRemove.Quantity == 0 )
                {
                    _cartItems.Remove(cartItemToRemove);
                }
            }
        }

        public virtual void ClearCart()
        {
            _cartItems.Clear();
        }

        
    }
}
