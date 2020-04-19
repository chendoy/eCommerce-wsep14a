using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreTest/>
    public class Store
    {
        private DiscountPolicy discountPolicy;
        private PuarchsePolicy puarchsePolicy;
        private Inventory inventory;
        private int rank;
        private bool activeStore;

        /// <summary>
        /// ONLY For generating Stubs
        /// </summary>
        public Store ()
        {
        }

        public Store(Dictionary<string, object> store_params)
        {
            this.Id = (int)store_params[CommonStr.StoreParams.StoreId];
            this.owners = new List<User>();
            User storeOwner = (User)store_params[CommonStr.StoreParams.mainOwner];
            this.owners.Add(storeOwner);
            this.managers = new List<User>();

            this.inventory = new Inventory();
            if (store_params.ContainsKey(CommonStr.StoreParams.StoreInventory))
                this.inventory = (Inventory)store_params[CommonStr.StoreParams.StoreInventory];
      
            this.discountPolicy = (DiscountPolicy)store_params[CommonStr.StoreParams.StoreDiscountPolicy];
            this.puarchsePolicy = (PuarchsePolicy)store_params[CommonStr.StoreParams.StorePuarchsePolicy];
            this.activeStore = true;

            if (store_params.ContainsKey(CommonStr.StoreParams.StoreRank))
                this.rank = (int)store_params[CommonStr.StoreParams.StoreRank];
            else
                this.rank = 1;

        }

        public Tuple<bool, string> IncreaseProductAmount(User user, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg + " user: " + user.getUserName().ToString() + "store: " + this.Id, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logError(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return inventory.IncreaseProductAmount(productId, amount);
        }

        public Tuple<bool, string> decrasePrdouctAmount(User user, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logError(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return inventory.DecraseProductAmount(productId, amount);
        }

        public Tuple<bool,string> changeStoreStatus(User user, bool newStatus)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                }
            }
            ActiveStore = newStatus;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logError(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return inventory.removeProduct(productId);
        }

        public Tuple<bool, string> appendProduct(User user, Dictionary<string, object> productParams, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logError(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);

                }
            }

            return inventory.appendProduct(productParams, amount);
        }



        public Tuple<bool, string> UpdateProduct(User user, Dictionary<string, object> productParams)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logError(CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logError(CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg, this, System.Reflection.MethodBase.GetCurrentMethod());
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return inventory.UpdateProduct(productParams);
        }

        public Tuple<bool, string> DecraseProductAmountAfterPuarchse(int productId, int amount)
        {
            return inventory.DecraseProductAmount(productId, amount);
        }
        public Tuple<bool, string> IncreaseProductAmountAfterFailedPuarchse(int productId, int amount)
        {
            return inventory.IncreaseProductAmount(productId, amount);
        }

        public Dictionary<string, object> getSotoreInfo()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<string, object> store_info = new Dictionary<string, object>();
            store_info.Add(CommonStr.StoreParams.StoreId, Id);
            if (owners.Count > 0)
                store_info.Add(CommonStr.StoreParams.mainOwner,owners[0]);
            store_info.Add(CommonStr.StoreParams.StoreInventory, inventory);
            store_info.Add(CommonStr.StoreParams.StoreDiscountPolicy, discountPolicy);
            store_info.Add(CommonStr.StoreParams.StorePuarchsePolicy, puarchsePolicy);
            store_info.Add(CommonStr.StoreParams.IsActiveStore, ActiveStore);
            store_info.Add(CommonStr.StoreParams.StoreRank, Rank);
            return store_info;
        }


        public bool AddStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners.Contains(user))
                return false;
            owners.Add(user);
            return true;
        }

        public bool AddStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (managers.Contains(user))
                return false;
            managers.Add(user);
            return true;
        }

        public bool IsStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return owners.Contains(user);
        }
        
        public bool IsStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return managers.Contains(user);
        }

        public bool RemoveManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return managers.Remove(user);
        }

        public bool RemoveOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return owners.Remove(user);
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }


        internal int getStoreId()
        {
            return Id;
        }


        public List<User> owners {
            // we dont check for correctn's because it's appoitnment responsibility
            set; get; }

        public List<User> managers {
            // we dont check for correctn's because it's appoitnment responsibility
            set; get; }

        public int Id { get; }

        public Inventory Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        public DiscountPolicy DiscountPolicy
        {
            get { return discountPolicy; }
            set { discountPolicy = value; }
        }

        public PuarchsePolicy PuarchsePolicy
        {
            get { return puarchsePolicy; }
            set { puarchsePolicy = value; }
        }
     
        public bool ActiveStore {
            get { return this.activeStore; }
            set { activeStore = value; } 
        }

        //return product and its amount in the inventory
        public Tuple<Product, int> getProductDetails(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return inventory.getProductDetails(productId);
        }

        public bool productExist(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return inventory.productExist(productId);
        }

        public double getBasketPrice(Dictionary<int, int> products)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            //TODO: manage the price also according in the discountPolicy in next version
            return inventory.getBasketPrice(products);
        }

        public Tuple<bool, string> checkIsValidBasket(Dictionary<int, int> products)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            //TODO: check if the basket is stands for the puarcshePolicy with puarchsepolicy object! next version
            return inventory.isValidBasket(products);
        }

        public bool isMainOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners[0] == user)
                return true;
            else
                return false;
        }
    }
}
