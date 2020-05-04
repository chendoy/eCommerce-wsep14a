using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreManagmentTest/>
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;
        private int nextStoreId = 0;
        private UserManager userManager = UserManager.Instance;
        private static StoreManagment instance = null;
        private static readonly object padlock = new object();

        /// <summary>
        /// Public ONLY For generatin Stubs
        /// </summary>
        public StoreManagment()
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

      

        public Tuple<bool, string> appendProduct(int storeId, string userName, int pId, string pDetails, double pPrice, string pName, string pCategory, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logError(CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg,this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            if (!stores.ContainsKey(storeId))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            }

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductId, pId);
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
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

            return stores[storeId].decrasePrdouctAmount(user:user , productId: productId, amount: amount);
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
        
        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
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
            
            return stores[storeId].UpdateProduct(user, productParams);
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





        public Tuple<int, string> createStore(string userName, int discountType, int puarchseType)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.userNotFoundErrMsg);
            }

            nextStoreId += 1;

            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            storeParam.Add(CommonStr.StoreParams.StoreId, nextStoreId);
            storeParam.Add(CommonStr.StoreParams.mainOwner, user);
            storeParam.Add(CommonStr.StoreParams.StoreDiscountPolicy, new DiscountPolicy(discountType));
            storeParam.Add(CommonStr.StoreParams.StorePuarchsePolicy, new PuarchsePolicy(puarchseType));
            Store store = new Store(storeParam);

            Tuple<bool, string> ownershipAdded = user.addStoreOwnership(store);

            if (!ownershipAdded.Item1)
            {
                nextStoreId -= 1;
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), ownershipAdded.Item2);
                return new Tuple<int, string>(-1, ownershipAdded.Item2);
            }
            else
            {
                stores.Add(nextStoreId, store);
                //Version 2 Addition
                Tuple<bool, string> ans = Publisher.Instance.subscribe(userName, nextStoreId);
                if (!ans.Item1)
                    return new Tuple<int, string>(-1, ans.Item2);
                return new Tuple<int, string>(nextStoreId, "");
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
            Tuple<bool,string> ans =  Publisher.Instance.Notify(storeId, new NotifyData("Store Closed by Main Owner"));
            if (!ans.Item1)
                return ans;
            if (!Publisher.Instance.RemoveSubscriptionStore(storeId))
                return new Tuple<bool, string>(false,"Cannot Remove Subscription Store");
            stores.Remove(storeId);
            return new Tuple<bool, string>(true, "");
        }


        // impl on next version only!
        private bool isMainOwner(User user, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return stores[storeId].isMainOwner(user);
        }


        public void setStores(Dictionary<int, Store> stores)
        {
            this.stores = stores;
        }

        public void cleanup()
        {
            this.stores = new Dictionary<int, Store>();
            this.nextStoreId = 0;
            userManager = UserManager.Instance;
        }
    }
}
