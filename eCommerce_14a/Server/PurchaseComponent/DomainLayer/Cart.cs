using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.DAL;
using Server.DAL.StoreDb;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{

    public class Cart
    {

        public int Id { get; set; }
        public string user { get; set; }
        public Dictionary<Store, PurchaseBasket> baskets { get; set; }
        public double Price { get; private set; }

        public bool IsPurchased { get; set; }

        public Cart(string user)
        {
            Id = DbManager.Instance.GetnextCartId();
            this.user = user;
            this.baskets = new Dictionary<Store, PurchaseBasket>();
            Price = 0;
            IsPurchased = false;
        }
        public Cart(int id, string username, double price, Dictionary<Store, PurchaseBasket> purchasebaskets, bool ispurchased)
        {
            user = username;
            Id = id;
            Price = price;
            baskets = purchasebaskets;
            IsPurchased = ispurchased;
        }

        internal Dictionary<Store, PurchaseBasket> GetBaskets()
        {
            return this.baskets;
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProduct(Store store, int productId, int wantedAmount, bool exist)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            if (!store.ProductExist(productId))
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

                //Inserting new basket To db
                DbManager.Instance.InsertPurchaseBasket(StoreAdapter.Instance.ToDbPurchseBasket(basket, this.Id));
            }

            Tuple<bool, string> res = basket.AddProduct(productId, wantedAmount, exist);

            if (basket.IsEmpty())
            {
                baskets.Remove(store);
                //Update DB delete purchase basket
                DbManager.Instance.DeletePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(basket.Id));
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
            //Update CART PRICE AT DB
            DbManager.Instance.UpdateDbCart(DbManager.Instance.GetDbCart(Id), this);
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

        // For tests
        public int GetAmountOfUniqueProducts()
        {
            int res = 0;
            foreach (var basket in baskets.Values)
            {
                res += basket.GetAmountOfUniqueProducts();
            }

            return res;
        }

        public PurchaseBasket GetBasket(Store store)
        {
            if (baskets.ContainsKey(store))
                return baskets[store];
            else
                return null;
        }
    }
}
