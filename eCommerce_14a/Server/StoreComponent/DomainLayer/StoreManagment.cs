﻿using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;
using Server.StoreComponent.DomainLayer;
using Server.Communication.DataObject.ThinObjects;
using eCommerce_14a.PurchaseComponent.DomainLayer;

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
        public Tuple<bool, string> appendProduct(int storeId, string userName, int pId, string pDetails, double pPrice, string pName, string pCategory, int amount, string imgUrl)
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
            productParams.Add(CommonStr.ProductParams.ProductImgUrl, imgUrl);
            return stores[storeId].appendProduct(user, productParams, amount);
        }
        public void LoadStores()
        {
            createStore("user4","Store1"); //id 1
            createStore("user5", "Store2"); //id 2
            //Products
            appendProduct(1, "user4", 1, "Banana", 7.5, "banana", CommonStr.ProductCategoty.Health, 80, @"Image/bana.png");
            appendProduct(2, "user5", 1, "Banana", 7.5, "banana", CommonStr.ProductCategoty.Health, 80, @"Image/bana.png");
            appendProduct(1, "user4", 2, "Choco", 5, "coco", CommonStr.ProductCategoty.Health, 70, @"Image/bana.png");
            appendProduct(2, "user5", 2, "Choco", 5, "coco", CommonStr.ProductCategoty.Health, 70, @"Image/bana.png");
            appendProduct(1, "user4", 3, "Moka", 10, "Moka", CommonStr.ProductCategoty.Health, 120, @"Image/bana.png");
            appendProduct(2, "user5", 3, "Moka", 10, "Moka", CommonStr.ProductCategoty.Health, 120, @"Image/bana.png");
            appendProduct(1, "user4", 4, "Apple", 40, "Apple", CommonStr.ProductCategoty.Health, 10, @"Image/bana.png");
            appendProduct(2, "user5", 4, "Apple", 40, "Apple", CommonStr.ProductCategoty.Health, 10, @"Image/bana.png");
            appendProduct(1, "user4", 5, "WaterMellon", 2, "WaterMellon", CommonStr.ProductCategoty.Health, 45, @"Image/bana.png");
            appendProduct(2, "user5", 5, "WaterMellon", 2, "WaterMellon", CommonStr.ProductCategoty.Health, 45, @"Image/bana.png");
            //Buying Policy
            PurchasePolicyData max1bBanansStore2 = new PurchasePolicyProductData(CommonStr.PurchasePreCondition.singleOfProductType, 1);
            PurchasePolicyData max10ItemsBasket = new PurchasePolicyBasketData(CommonStr.PurchasePreCondition.Max10ProductPerBasket);
            List<PurchasePolicyData> childrenPurchase = new List<PurchasePolicyData>();
            childrenPurchase.Add(max1bBanansStore2);
            childrenPurchase.Add(max10ItemsBasket);
            PurchasePolicyData purchasePolicyData = new CompoundPurchasePolicyData(CommonStr.PurchaseMergeTypes.AND, childrenPurchase);
            UpdatePurchasePolicy(2, "user5", purchasePolicyData);
            //Discount Policy

            DiscountPolicyData discountPerProductAbove1Unit_1 = new DiscountConditionalProductData(3,CommonStr.DiscountPreConditions.Above1Unit, 10.0);
            DiscountPolicyData discountPerProductAbove2Unit_1 = new DiscountConditionalProductData(3, CommonStr.DiscountPreConditions.Above2Units, 15.0);
            List<DiscountPolicyData> Units_children = new List<DiscountPolicyData>();
            Units_children.Add(discountPerProductAbove1Unit_1);
            Units_children.Add(discountPerProductAbove2Unit_1);
            DiscountPolicyData discount_xor_aboveUnits = new CompoundDiscountPolicyData(CommonStr.DiscountMergeTypes.XOR, Units_children);
            DiscountPolicyData basketDiscount = new DiscountConditionalBasketData(CommonStr.DiscountPreConditions.basketPriceAbove1000, 10.0);
            List<DiscountPolicyData> discountAllChildren = new List<DiscountPolicyData>();
            discountAllChildren.Add(discount_xor_aboveUnits);
            discountAllChildren.Add(basketDiscount);
            DiscountPolicyData discountPolicyfinal = new CompoundDiscountPolicyData(CommonStr.DiscountMergeTypes.AND, discountAllChildren);
            UpdateDiscountPolicy(2, "user5", discountPolicyfinal);
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
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.GuestCantBuy, "Guest can't Buy");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.Max10ProductPerBasket, "Max 10 product per basket!");
            avilablePurchasePolicy.Add(CommonStr.PurchasePreCondition.singleOfProductType, "you can buy only single unit of product type");
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


        public Tuple<bool, string> UpdateDiscountPolicy(int storeId, string userName, string discountPolicyData)
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
            return stores[storeId].UpdateDiscountPolicy(user, discountPolicyData);

        }


        public Tuple<bool, string> UpdatePurchasePolicy(int storeId, string userName, PurchasePolicyData purchasePolicyData)
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
            return stores[storeId].UpdatePurchasePolicy(user, purchasePolicyData);

        }

        internal List<Store> GetStoresOwnedBy(string username)
        {
            List<Store> retList = new List<Store>();
            List<Store> allStores = stores.Values.ToList();
            foreach (Store store in allStores) 
            {
                List<User> owners = store.owners;
                foreach (User user in owners) 
                {
                    if (user.getUserName().Equals(username))
                        retList.Add(store);
                }

                foreach (User user in store.managers)
                {
                    if (user.getUserName().Equals(username))
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

            nextStoreId += 1;

            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            storeParam.Add(CommonStr.StoreParams.StoreId, nextStoreId);
            storeParam.Add(CommonStr.StoreParams.StoreName, storename);
            storeParam.Add(CommonStr.StoreParams.mainOwner, user);
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
            Tuple<bool,string> ans =  Publisher.Instance.Notify(storeId, new NotifyData("Store Closed by Main Owner - "+userName));
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
