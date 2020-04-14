using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    public class Cart
    {
        private string user;
        private Dictionary<Store, PurchaseBasket> baskets;
        private int price;

        public Cart(string user)
        {
            this.user = user;
            this.baskets = new Dictionary<Store, PurchaseBasket>();
            this.price = 0;
        }

        public Dictionary<Store, PurchaseBasket> GetBaskets()
        {
            return this.baskets;
        }

        public int GetPrice()
        {
            return this.price;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(Store store, int productId, int wantedAmount, bool exist)
        {
            if (store == null)
            {
                return new Tuple<bool, string>(false, "Invalid store");
            }

            if (!baskets.TryGetValue(store, out PurchaseBasket basket))
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, "The product is not already in the shopping cart");
                }

                basket = new PurchaseBasket(this.user, store);
                baskets.Add(store, basket);
            }

            Tuple<bool, string> res =  basket.AddProduct(productId, wantedAmount, exist);
            UpdateCartPrice();
            return res;
        }

        private void UpdateCartPrice()
        {
            int price = 0;
            foreach (var basket in baskets.Values)
            {
                price += basket.GetPrice();
            }
        }
    }
}
