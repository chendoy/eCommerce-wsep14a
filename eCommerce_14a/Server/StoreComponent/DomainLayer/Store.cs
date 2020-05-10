using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreTest/>
    public class Store
    {
        private DiscountPolicy discountPolicy;
        private PurchasePolicy purchasePolicy;
        private Validator policyValidator;
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
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StoreInventory) || store_params[CommonStr.StoreParams.StoreInventory] == null)
            {
                this.inventory = new Inventory();
            }
            else
            {
                this.inventory = (Inventory)store_params[CommonStr.StoreParams.StoreInventory];
            }


            if (!store_params.ContainsKey(CommonStr.StoreParams.Validator) || store_params[CommonStr.StoreParams.Validator] == null)
            {
                policyValidator = new Validator(null, null);
                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.allwaysTrue,
                (PurchaseBasket basket, int productId, User user, Store store) => true);
                policyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.NoDiscount,
                    (PurchaseBasket basket, int productId) => true);
            }
            else
            {
                this.policyValidator = (Validator)store_params[CommonStr.StoreParams.Validator];
            }
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StoreDiscountPolicy) || store_params[CommonStr.StoreParams.StoreDiscountPolicy] == null)
            {
                this.discountPolicy = new ConditionalBasketDiscount(new PreCondition(CommonStr.DiscountPreConditions.NoDiscount), 0);
            }
            else
            {
                this.discountPolicy = (DiscountPolicy)store_params[CommonStr.StoreParams.StoreDiscountPolicy];
            }
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StorePuarchsePolicy) || store_params[CommonStr.StoreParams.StorePuarchsePolicy] == null)
            {
                this.purchasePolicy = purchasePolicy = new BasketPurchasePolicy(new PurchasePreCondition(CommonStr.PurchasePreCondition.allwaysTrue));
            }
            else
            {
                this.purchasePolicy = (PurchasePolicy)store_params[CommonStr.StoreParams.StorePuarchsePolicy];
            }

            if (!store_params.ContainsKey(CommonStr.StoreParams.StoreRank) || store_params[CommonStr.StoreParams.StoreRank] == null)
            {
                this.rank = 1;
            }
            else
            {
                this.rank = (int)store_params[CommonStr.StoreParams.StoreRank];
            }

            this.activeStore = true;
        }

  
        public Tuple<bool, string> IncreaseProductAmount(User user, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg + " user: " + user.getUserName().ToString() + "store: " + this.Id);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
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
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
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
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                }
            }
            ActiveStore = newStatus;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> UpdateDiscountPolicy(User user, DiscountPolicy discountPolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.DiscountPolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }
            this.discountPolicy = discountPolicy;
            return new Tuple<bool, string>(true,"");
        }

        public Tuple<bool, string> UpdatePurchasePolicy(User user, PurchasePolicy purchasePolicy)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.PurachsePolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            this.purchasePolicy = purchasePolicy;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user) && !managers.Contains(user))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
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
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
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
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                   
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
            store_info.Add(CommonStr.StoreParams.StorePuarchsePolicy, purchasePolicy);
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

        public Validator PolicyValidator
        {
            set { this.policyValidator = value; }
            get { return policyValidator; }
        }

  

        public DiscountPolicy DiscountPolices
        {
            get { return discountPolicy; }
        }

        public PurchasePolicy PurchasePolicy
        {
            get { return purchasePolicy; }
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

        virtual
        public double getBasketPrice(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            double basketPrice = inventory.getBasketPrice(basket.Products);
            double overallDiscount = discountPolicy.CalcDiscount(basket, policyValidator);
            double priceAfterDiscount = basketPrice - overallDiscount;

            return priceAfterDiscount;
        }

        public Tuple<bool, string> checkIsValidBasket(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            //checks if the basket accomodate the store's purchase policy
            if (!purchasePolicy.IsEligiblePurchase(basket, policyValidator))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
            }
            return inventory.isValidBasket(basket.Products);
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
