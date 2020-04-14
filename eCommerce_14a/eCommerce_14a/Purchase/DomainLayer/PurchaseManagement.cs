using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    public class PurchaseManagement
    {
        private Dictionary<string, Cart> carts;

        public PurchaseManagement()
        {
            this.carts = new Dictionary<string, Cart>();
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// Get the user ,store and product to add to the shopping cart
        /// Additionaly indicate how much items of the product should be in the cart
        /// exist - means this product meant to be already in the cart (in case of change/remove existing product
        public Tuple<bool, string> AddProductToShoppingCart(string user, string store, string product, int wantedAmount, bool exist)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<bool, string>(false, "Not a valid user");
            }

            if (!External.CheckValidProduct(store, product))
            {
                return new Tuple<bool, string>(false, "Not a valid product");
            }

            if (wantedAmount < 0)
            {
                return new Tuple<bool, string>(false, "Cannot have negative amount of product");
            }

            if (!exist && wantedAmount == 0)
            {
                return new Tuple<bool, string>(false, "Cannot add product to cart with zero amount");
            }

            int amount = External.GetAmountOfProduct(store, product);

            if (amount < wantedAmount)
            {
                return new Tuple<bool, string>(false, "There is not enough products in the store");
            }

            if (!this.carts.TryGetValue(user, out Cart cart))
            {
                cart = new Cart(user);
                carts.Add(user, cart);
            }

            return cart.AddProduct(store, product, wantedAmount, exist);
        }

        public Tuple<Dictionary<string, PurchaseBasket>, string> GetCartDetails(string user)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<Dictionary<string, PurchaseBasket>, string>(null, "Not a valid user");
            }
            if (!carts.TryGetValue(user, out Cart cart))
            {
                return new Tuple<Dictionary<string, PurchaseBasket>, string>(null, "No cart found for this user");
            }

            return new Tuple<Dictionary<string, PurchaseBasket>, string>(cart.GetBaskets(), "");
        }

    }
}
