using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;
using Server.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using Server.DAL;
using Server.Utils;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreManagmentTest/>
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;
        private UserManager userManager = UserManager.Instance;
        private static StoreManagment instance = null;
        private static readonly object padlock = new object();

        /// <summary>
        /// Public ONLY For generatin Stubs
        /// </summary>
        private StoreManagment()
        {
            stores = new Dictionary<int, Store>();
        }

        public static StoreManagment Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StoreManagment();
                    }
                    return instance;
                }
            }
        }


        public List<Store> GetAllStores() 
        {
            List<Store> retList = new List<Store>();
            foreach (Store store in stores.Values.ToList()) 
            {
                if (store.ActiveStore)
                    retList.Add(store);
            }
            return retList;
        }
      
        public void LoadFromDb()
        {
            List<Store> allstores = DbManager.Instance.LoadAllStores();
            foreach(Store store in allstores)
            {
                stores.Add(store.Id, store);
                Publisher.Instance.subscribe(store.owners[0], store.Id);
            }
          
        }

        public Dictionary<int, string> GetAvilableRawDiscount()
        {
            Dictionary<int, string> avilableDiscount = new Dictionary<int, string>();
            avilableDiscount.Add(CommonStr.DiscountPreConditions.NoDiscount, "no discount at all");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.Above1Unit, "buy more Than 1 unit and get discount");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.Above2Units, "buy more than 2 units and get discount");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.ProductPriceAbove100, "if product price above 100, get discount");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.ProductPriceAbove200, "if product price above 200, get discount");
            avilableDiscount.Add(CommonStr.DiscountPreConditions.basketPriceAbove1000, "if basket price above 1000, get discount");
            return avilableDiscount;
        }

        public Dictionary<int, string> GetAvilableRawPurchasePolicy()
        {
            Dictionary<int, string> avilablePurchasePolicy = new Dictionary<int, string>();
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.allwaysTrue, "No Condition");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.OwnerCantBuy, "Strore Owner Can't buy from the store");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.Max10ProductPerBasket, "Max 10 product per basket!");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.AtLeat11ProductPerBasket, "At Leat 11 product per basket");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.singleOfProductType, "you can buy only single unit of product id");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.StoreMustBeActive, "in order to buy store must be active");
            return avilablePurchasePolicy;
        }



        public  Dictionary<string, string> GetStaffStroe(int storeID)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!stores.ContainsKey(storeID))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Dictionary<string, string>();
            }
            return stores[storeID].getStaff();
        }

        public Tuple<bool, string> appendProduct(int storeId, string userName, string pDetails, double pPrice, string pName, string pCategory, int amount, string imgUrl)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            productParams.Add(CommonStr.ProductParams.ProductImgUrl, imgUrl);
            return stores[storeId].appendProduct(user, productParams, amount);
        }


        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].removeProduct(user, productId);
        }

        internal string GetDiscountPolicy(int storeID)
        {
            Store store;
            if (!stores.TryGetValue(storeID, out store))
                return "";
            return store.DiscountPolicy.Describe(0);
        }

        internal string GetPurchasePolicy(int storeID)
        {
            Store store;
            if (!stores.TryGetValue(storeID, out store))
                return "";
            return store.PurchasePolicy.Describe(0);
        }

        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory, string imgUrl)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductId, productId);
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            productParams.Add(CommonStr.ProductParams.ProductImgUrl, imgUrl);


            return stores[storeId].UpdateProduct(user, productParams);
        }

        internal List<Store> GetStoresOwnedBy(string username)
        {
            List<Store> retList = new List<Store>();
            List<Store> allStores = stores.Values.ToList();
            foreach (Store store in allStores) 
            {
                List<string> owners = store.owners;
                foreach (string user in owners) 
                {
                    if (user.Equals(username))
                        retList.Add(store);
                }

                foreach (string user in store.managers)
                {
                    if (user.Equals(username))
                        retList.Add(store);
                }
            }
            return retList;
        }

        public Tuple<bool, string> addProductAmount(int storeId, string userName, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].IncreaseProductAmount(user: user, productId: productId, amount: amount);
        }

        public Tuple<bool, string> decraseProductAmount(int storeId, string userName, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].decrasePrdouctAmount(user: user, productId: productId, amount: amount);
        }


        public Tuple<bool, string> UpdateDiscountPolicy(int storeId, string userName, string discountPolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }
            DiscountPolicy parsedDiscount = DiscountParser.Parse(discountPolicy);
            if(!DiscountParser.checkDiscount(parsedDiscount))
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.DiscountPolicyParsedFailed);
            }
            return stores[storeId].UpdateDiscountPolicy(user, parsedDiscount);

        }


        public Tuple<bool, string> UpdatePurchasePolicy(int storeId, string userName, string purchasePolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }
            PurchasePolicy parsedPurchase = PurchasePolicyParser.Parse(purchasePolicy);
            if(!PurchasePolicyParser.CheckPurchasePolicy(parsedPurchase))
            {
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.PurchasePolicyParsedFailed);

            }
            return stores[storeId].UpdatePurchasePolicy(user, parsedPurchase);

        }

     

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Dictionary<string, object>();
            }

            return stores[storeId].getSotoreInfo();
        }


     
        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            return stores[storeId].changeStoreStatus(user, status);

        }
        
       


        public virtual Store getStore(int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (stores.ContainsKey(storeId))
                return stores[storeId];
            return null;
        }

        public Dictionary<int, Store> getActiveSotres()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<int, Store> activeStores = new Dictionary<int, Store>();
            foreach (KeyValuePair<int, Store> storeEntry in stores)
            {
                if (storeEntry.Value.ActiveStore)
                {
                    activeStores.Add(storeEntry.Key, storeEntry.Value);
                }

            }
            return activeStores;
        }





        public Tuple<int, string> createStore(string userName, string storename)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }
            if(storename == null || storename.Equals(""))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.illegalStoreName);
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.illegalStoreName);
            }


            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            int next_id = DbManager.Instance.GetNextStoreId();
            storeParam.Add(CommonStr.StoreParams.StoreId, next_id);
            storeParam.Add(CommonStr.StoreParams.StoreName, storename);
            storeParam.Add(CommonStr.StoreParams.mainOwner, user.Name);
            Store store = new Store(storeParam);
            //DB Insert Store
            DbManager.Instance.InsertStore(store);

            Tuple<bool, string> ownershipAdded = user.addStoreOwnership(store.Id,userName);

            if (!ownershipAdded.Item1)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), ownershipAdded.Item2);
                DbManager.Instance.DeleteFullStore(store);
                return new Tuple<int, string>(-1, ownershipAdded.Item2);
            }
            else
            {        
                stores.Add(next_id, store);
                
                //Version 2 Addition
                Tuple<bool, string> ans = Publisher.Instance.subscribe(userName, next_id);
                if (!ans.Item1)
                    return new Tuple<int, string>(-1, ans.Item2);
                return new Tuple<int, string>(next_id, "");
            }

        }


        // impl on next version only!
        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }


            if (!isMainOwner(user, storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.notMainOwnerErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.notMainOwnerErrMessage);
            }


            if (!user.isStoreOwner(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.notStoreOwnerErrMessage + "- user: " + userName + " store id:" + storeId.ToString());
                return new Tuple<bool, string>(false, "user" + userName + " not store owner of " + storeId.ToString());
            }

            Tuple<bool, string> ownerShipedRemoved = userManager.removeAllFromStore(storeId);
            if (!ownerShipedRemoved.Item1)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), ownerShipedRemoved.Item2);
                return ownerShipedRemoved;
            }
            //Version 2 Addition
            Tuple<bool,string> ans =  Publisher.Instance.Notify(storeId, new NotifyData("Store Closed by Main Owner - "+userName));
            if (!ans.Item1)
                return ans;
            if (!Publisher.Instance.RemoveSubscriptionStore(storeId))
                return new Tuple<bool, string>(false,"Cannot Remove Subscription Store");

            //DB addition 
            DbManager.Instance.DeleteFullStore(stores[storeId]);
            stores.Remove(storeId);
            return new Tuple<bool, string>(true, "");
        }


        // impl on next version only!
        private bool isMainOwner(User user, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return stores[storeId].IsMainOwner(user);
        }


        public void setStores(Dictionary<int, Store> stores)
        {
            this.stores = stores;
        }

        public void cleanup()
        {
            this.stores = new Dictionary<int, Store>();
            userManager = UserManager.Instance;
        }
    }
}
