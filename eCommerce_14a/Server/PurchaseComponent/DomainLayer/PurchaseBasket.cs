using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL;
using Server.DAL.StoreDb;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class PurchaseBasket
    {

        public int Id { get; set; }
        public string User { get; set; }

        public  Store store { get; set; }
        public Dictionary<int, int> products { get; set; }
        public double Price { get; set; }
        public DateTime? PurchaseTime { get; private set; }

        public PurchaseBasket(string user, Store store)
        {
            Id = DbManager.Instance.GetNextPurchBasketId();
            this.User = user;
            this.store = store;
            this.products = new Dictionary<int, int>();
            Price = 0;
            PurchaseTime = null;
        }

        public PurchaseBasket(int id, string user, Store store, Dictionary<int, int> products, double price, DateTime? purchaseTime)
        {
            Id = id;
            User = user;
            this.store = store;
            this.products = products;
            Price = price;
            PurchaseTime = purchaseTime;
        }

        public override string ToString()
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
            if (!this.Store.ProductExist(productId))
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
                    //DB Delete Product From Basekt
                    DbManager.Instance.DeletePrdocutAtBasket(DbManager.Instance.GetProductAtBasket(this.Id, productId));
                }
                else
                {
                    products[productId] = wantedAmount;
                    //DB Update product amount at basket!
                    DbManager.Instance.UpdateProductAtBasket(DbManager.Instance.GetProductAtBasket(this.Id, productId), wantedAmount);
                }
            }
            else
            {
                if (exist)
                {
                    return new Tuple<bool, string>(false, CommonStr.PurchaseMangmentErrorMessage.ProductNotExistInCartErrMsg);
                }

                products.Add(productId, wantedAmount);
                // DB Insert Product At Basket
                DbManager.Instance.InsertProductAtBasket(StoreAdapter.Instance.ToProductAtBasket(this.Id, productId, wantedAmount, this.Store.Id));
            }

            Tuple<bool, string> isValidBasket = Store.CheckIsValidBasket(this);
            if (!isValidBasket.Item1)
            {
                products = existingProducts;
                return isValidBasket;
            }

            Price = Store.GetBasketPriceWithDiscount(this);

            // DB Updating basket Price
            DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this);
            return new Tuple<bool, string>(true, null);
        }

        internal bool IsEmpty()
        {
            return products.Count == 0;
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281</req>
        internal double UpdateCartPrice()
        {
            Price = Store.GetBasketPriceWithDiscount(this);
            // DB update purchase BasketPrice
            DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this);
            return Price;
        }

        internal void SetPurchaseTime(DateTime purchaseTime)
        {
            PurchaseTime = purchaseTime;

            //UPDATING purchase time in db
            DbManager.Instance.UpdatePurchaseBasket(DbManager.Instance.GetDbPurchaseBasket(this.Id), this);
        }

        internal void RemoveFromStoreStock()
        {
            foreach (var product in products.Keys)
            {
                Store.DecraseProductAmountAfterPuarchse(product, products[product]);
            }
        }

        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchase-product-28</req>
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282</req>
        internal Tuple<bool, string> CheckProductsValidity()
        {
            if (Store == null || !Store.ActiveStore)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return Store.CheckIsValidBasket(this);
        }

        // For tests
        internal int GetAmountOfUniqueProducts()
        {
            return products.Keys.Count;
        }

        public Store Store
        {
            get { return Store; }
        }
        public double GetBasketPriceWithDiscount()
        {
            return Store.GetBasketPriceWithDiscount(this);
        }
        public double GetBasketOrigPrice()
        {
            return Store.GetBasketOrigPrice(this);
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
                Store.IncreaseProductAmountAfterFailedPuarchse(product, products[product]);
            }
        }
    }
}
