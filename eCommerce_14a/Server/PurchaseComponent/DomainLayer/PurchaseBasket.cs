using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseBasket
    {
        public string user { get; set; }
        public  Store store { get; set; }
        public Dictionary<int, int> products { get; set; }
        public double Price { get; set; }
        public DateTime PurchaseTime { get; private set; }

        public PurchaseBasket(string user, Store store)
        {
            this.user = user;
            this.store = store;
            this.products = new Dictionary<int, int>();
            Price = 0;
        }

        public string ToString()
        {
            string products_name = "";
            foreach (var product in products.Keys)
            {
                products_name += "Product ID - " + product + " Product Amount - " + products[product];
            }
            return products_name;
        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        /// This method Add/Change/Remove product from this basket
        /// <param name="exist">state if it should be already in the basket</param>
        virtual
        public Tuple<bool, string> AddProduct(int productId, int wantedAmount, bool exist)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!this.store.productExist(productId))
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            }

            Dictionary<int, int> existingProducts = new Dictionary<int, int>(products);
            if (products.ContainsKey(productId))
            {
                if (!exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProductExistInCartErrMsg);
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
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProductNotExistInCartErrMsg);
                }

                products.Add(productId, wantedAmount);
            }

            Tuple<bool, string> isValidBasket = store.checkIsValidBasket(this);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                return isValidBasket;
            }

            Price = store.getBasketPriceWithDiscount(this);

            return new Tuple<bool, string>(true, null);
        }

        internal bool IsEmpty()
        {
            return products.Count == 0;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        internal double UpdateCartPrice()
        {
            Price = store.getBasketPriceWithDiscount(this);
            return Price;
        }

        internal void SetPurchaseTime(DateTime purchaseTime)
        {
            PurchaseTime = purchaseTime;
        }

        internal void RemoveFromStoreStock()
        {
            foreach (var product in products.Keys)
            {
                store.DecraseProductAmountAfterPuarchse(product, products[product]);
            }
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            if (store == null || !store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return store.checkIsValidBasket(this);
        }

        // For tests
        internal int GetAmountOfUniqueProducts()
        {
            return products.Keys.Count;
        }

        public Store Store
        {
            get { return store; }
        }
        public double GetBasketPriceWithDiscount()
        {
            return store.getBasketPriceWithDiscount(this);
        }
        public double GetBasketOrigPrice()
        {
            return store.getBasketOrigPrice(this);
        }

        public double getBasketDiscount()
        {
            return GetBasketOrigPrice() - GetBasketPriceWithDiscount();
        }

        public int GetNumProductsAtBasket()
        {
            int numProducts = 0;
            foreach(KeyValuePair<int, int> entry in products)
            {
                numProducts += entry.Value;
            }
            return numProducts;
        }
        public Dictionary<int, int> Products
        {
            get { return products; }
        }
        internal void RestoreItemsToStore()
        {
            foreach (var product in products.Keys)
            {
                store.IncreaseProductAmountAfterFailedPuarchse(product, products[product]);
            }
        }
    }
}
