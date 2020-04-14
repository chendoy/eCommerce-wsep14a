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
        public Tuple<bool, string> AddProduct(string product, int wantedAmount, bool exist)
        {
            if (products.ContainsKey(product))
            {
                if (!exist)
                {
                    return new Tuple<bool, string>(false, "The product is already in the shopping cart");
                }

                if (wantedAmount == 0)
                {
                    products.Remove(product);
                }
                else
                {
                    products[product] = wantedAmount;
                }
            }
            else
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, "The product is not already in the shopping cart");
                }

                products.Add(product, wantedAmount);
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
