using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    class Cart
    {
        private string user;
        private Dictionary<string, PurchaseBasket> baskets;

        public Cart(string user)
        {
            this.user = user;
            this.baskets = new Dictionary<string, PurchaseBasket>();
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(string store, string product, int wantedAmount)
        {
            if (!External.CheckValidStore(store))
            {
                return new Tuple<bool, string>(false, "Invalid store");
            }

            if (!baskets.TryGetValue(store, out PurchaseBasket basket))
            {
                basket = new PurchaseBasket(this.user, store);
                baskets.Add(store, basket);
            }

            return basket.AddProduct(product, wantedAmount);
        }
    }
}
