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
        private Store store;
        private Dictionary<int, int> products;
        private int price;

        public PurchaseBasket(string user, Store store)
        {
            this.user = user;
            this.store = store;
            this.products = new Dictionary<int, int>();
            this.price = 0;
        }

        public int GetPrice()
        {
            return this.price;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(int productId, int wantedAmount, bool exist)
        {
            Dictionary<int, int> existingProducts = new Dictionary<int, int>(products);
            if (products.ContainsKey(productId))
            {
                if (!exist)
                {
                    return new Tuple<bool, string>(false, "The product is already in the shopping cart");
                }

                if (wantedAmount == 0)
                {
                    products.Remove(productId);
                }
                else
                {
                    products[productId] = wantedAmount;
                }
            }
            else
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, "The product is not already in the shopping cart");
                }

                products.Add(productId, wantedAmount);
            }

            Tuple<bool, string> isValidBasket = store.CheckValidBasket(products);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                return isValidBasket;
            }

            this.price = store.GetBasketPrice(products);

            return new Tuple<bool, string>(true, null);
        }
    }
}
