using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneWorks.Models;
using Newtonsoft.Json;

namespace DroneWorks.Models
{
    public class Cart
    {
        // a list collection to hold cart item objects

        [JsonProperty] private List<CartItem> cartItems = new List<CartItem>();

        // setting the maximum order quantity to 20
        
        const int MaxQuantity = 20;

        public void AddItem(Products aProduct)
        {
            CartItem aItem = cartItems.Where(p => p.Product.ProdPk == aProduct.ProdPk).FirstOrDefault();

            // If it is a new item

            if (aItem == null)
            {
                cartItems.Add(new CartItem { Product = aProduct, Quantity = 1 });
            }

            else
            {
                // Increase quantity by 1 if the current quantity is less than 20

                if (aItem.Quantity < MaxQuantity)
                {
                    aItem.Quantity += 1;
                }
            }
        }

        public void UpdateItem(Products aProduct, int quantity)
        {
            CartItem aItem = cartItems.Where(p => p.Product.ProdPk == aProduct.ProdPk).FirstOrDefault();

            if (aItem != null)
            {
                aItem.Quantity = (quantity <= MaxQuantity) ? quantity : MaxQuantity;
            }
        }

        public void RemoveItem(Products aProduct)
        {
            cartItems.RemoveAll(r => r.Product.ProdPk == aProduct.ProdPk);
        }

        public void ClearCart()
        {
            cartItems.Clear();
        }

        public decimal ComputeOrderTotal()
        {
            return cartItems.Sum(s => Convert.ToDecimal(s.Product.ProdPrice) * s.Quantity);
        }

        public IEnumerable<CartItem> CartItems()
        {
            return cartItems;
        }
    }
}
