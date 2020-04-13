using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    public class PurchaseFacade
    {
        private Dictionary<string, Cart> carts;

        public PurchaseFacade()
        {
            this.carts = new Dictionary<string, Cart>();
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProductToShoppingCart(string user, string store, string product, int wantedAmount)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<bool, string>(false, "Not a valid user");
            }

            if (!External.CheckValidProduct(store, product))
            {
                return new Tuple<bool, string>(false, "Not a valid product");
            }

            int amount = External.GetAmountOfProduct(store, product);

            if (amount < wantedAmount)
            {
                string error = String.Format("There is not enough products in the store, Wanted: {0} Exist: {1}",
                    wantedAmount, amount);
                return new Tuple<bool, string>(false, error);
            }

            if (!this.carts.TryGetValue(user, out Cart cart))
            {
                cart = new Cart(user);
                carts.Add(user, cart);
            }

            return cart.AddProduct(store, product, wantedAmount);
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

        public Tuple<bool, string> ClearUserCart(string user)
        {
            if (!External.CheckValidUser(user))
            {
                return new Tuple<bool, string>(false, "Not a valid user");
            }

            if (!carts.ContainsKey(user))
            {
                return new Tuple<bool, string>(false, "No cart found");
            }

            carts[user] = null;
            return new Tuple<bool, string>(true, null);
        }
    }
}
