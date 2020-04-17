using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseBasket
    {
        private string user;
        private readonly Store store;
        private Dictionary<int, int> products;
        private double Price { get; set; }
        public DateTime PurchaseTime { get; private set; }

        public PurchaseBasket(string user, Store store)
        {
            this.user = user;
            this.store = store;
            this.products = new Dictionary<int, int>();
            Price = 0;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        /// This method Add/Change/Remove product from this basket
        /// <param name="exist">state if it should be already in the basket</param>
        public Tuple<bool, string> AddProduct(int productId, int wantedAmount, bool exist)
        {
            if (!this.store.productExist(productId))
            {
                return new Tuple<bool, string>(false, "Not a valid product");
            }

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

            Tuple<bool, string> isValidBasket = store.checkIsValidBasket(products);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                return isValidBasket;
            }

            Price = store.getBasketPrice(products);

            return new Tuple<bool, string>(true, null);
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        internal double UpdateCartPrice()
        {
            Price = store.getBasketPrice(products);
            return Price;
        }

        internal void SetPurchaseTime(DateTime purchaseTime)
        {
            PurchaseTime = purchaseTime;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, "Invalid store");
            }

            return store.checkIsValidBasket(products);
        }
    }
}
