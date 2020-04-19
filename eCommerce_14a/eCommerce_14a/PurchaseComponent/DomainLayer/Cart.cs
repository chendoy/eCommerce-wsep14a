using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class Cart
    {
        private string user;
        private Dictionary<Store, PurchaseBasket> baskets;
        public double Price { get; private set; }

        public Cart(string user)
        {
            this.user = user;
            this.baskets = new Dictionary<Store, PurchaseBasket>();
            Price = 0;
        }

        internal Dictionary<Store, PurchaseBasket> GetBaskets()
        {
            return this.baskets;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(Store store, int productId, int wantedAmount, bool exist)
        {
            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!store.productExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            if (!baskets.TryGetValue(store, out PurchaseBasket basket))
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProductNotExistInCartErrMsg);
                }

                basket = new PurchaseBasket(this.user, store);
                baskets.Add(store, basket);
            }

            Tuple<bool, string> res = basket.AddProduct(productId, wantedAmount, exist);
            if (basket.IsEmpty())
            {
                baskets.Remove(store);
            }
            UpdateCartPrice();
            return res;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        internal void UpdateCartPrice()
        {
            Price = 0;
            foreach (var basket in baskets.Values)
            {
                Price += basket.UpdateCartPrice();
            }
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            foreach (var basket in baskets.Values)
            {
                Tuple<bool, string> res = basket.CheckProductsValidity();
                if (!res.Item1)
                {
                    return res;
                }
            }

            return new Tuple<bool, string>(true, "");
        }

        internal void SetPurchaseTime(DateTime purchaseTime)
        {
            foreach (var basket in baskets.Values)
            {
                basket.SetPurchaseTime(purchaseTime);
            }
        }

        public bool IsEmpty()
        {
            return baskets.Count == 0;
        }

        internal void RemoveFromStoresStock()
        {
            foreach (var basket in baskets.Values)
            {
                basket.RemoveFromStoreStock();
            }
        }

        internal void RestoreItemsToStores()
        {
            foreach (var basket in baskets.Values)
            {
                basket.RestoreItemsToStore();
            }
        }
    }
}
