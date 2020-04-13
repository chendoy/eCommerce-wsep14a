using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    class PurchaseBasket
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
        public Tuple<bool, Exception> AddProduct(string product, int wantedAmount)
        {
            if (!products.TryGetValue(product, out int amount))
            {
                amount = 0;
                products.Add(product, amount);
            }

            int currentWanted = wantedAmount + amount;

            int amountInStore = External.GetAmountOfProduct(store, product);

            if (amountInStore < currentWanted)
            {
                string error = String.Format("The is not enough products in the store summed up with your current basket, Wanted: {0} Exist: {1}",
                    currentWanted, amount);
                return new Tuple<bool, Exception>(false, new Exception(error));
            }

            products[product] = currentWanted;
            return new Tuple<bool, Exception>(true, null);
        }
    }
}
