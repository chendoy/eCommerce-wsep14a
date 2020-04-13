using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    public class PurchaseBasket
    {
        private string user;
        private string store;
        private Dictionary<string, int> products;

        public PurchaseBasket(string user, string store)
        {
            this.user = user;
            this.store = store;
            this.products = new Dictionary<string, int>();
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(string product, int wantedAmount)
        {
            if (wantedAmount == 0)
            {
                products.Remove(product);
            }
            else
            {
                if (!products.ContainsKey(product))
                {
                    products.Add(product, 0);
                }

                products[product] = wantedAmount;
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
