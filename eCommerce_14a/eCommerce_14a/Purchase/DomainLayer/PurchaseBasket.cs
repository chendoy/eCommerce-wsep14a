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
        private int price;

        public PurchaseBasket(string user, string store)
        {
            this.user = user;
            this.store = store;
            this.products = new Dictionary<string, int>();
            this.price = 0;
        }

        public int GetPrice()
        {
            return this.price;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(string product, int wantedAmount, bool exist)
        {
            Dictionary<string, int> existingProducts = new Dictionary<string, int>(products);
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

            Tuple<bool, string> isValidBasket = External.CheckValidBasketForStore(store, products);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                return isValidBasket;
            }

            this.price = External.GetBasketPrice(store, products);

            return new Tuple<bool, string>(true, null);
        }
    }
}
