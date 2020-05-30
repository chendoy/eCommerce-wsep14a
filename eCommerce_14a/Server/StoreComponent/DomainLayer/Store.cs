using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.Communication.DataObject.ThinObjects;
using Server.DAL;
using Server.DAL.StoreDb;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    /// <testclass cref ="TestingSystem.UnitTests.StoreTest/>
    public class Store
    {

        public int Id { set; get; }

        public DiscountPolicy DiscountPolicy { set; get; }
        public PurchasePolicy PurchasePolicy { set; get; }

        public PolicyValidator PolicyValidator { set; get; }

        public Inventory Inventory { set; get; }

        public int Rank { set; get; }

        public string StoreName { set; get; }


        public bool ActiveStore { set; get; }

        public List<string> owners{ set; get; }

        public List<string> managers{ set; get; }


        /// <summary>
        /// ONLY For generating Stubs
        /// </summary>
        public Store ()
        {
        }

        public Store(Dictionary<string, object> store_params)
        {
            Id = (int)store_params[CommonStr.StoreParams.StoreId];
            StoreName = (string)store_params[CommonStr.StoreParams.StoreName];
            owners = new List<string>();
            if (store_params.ContainsKey(CommonStr.StoreParams.mainOwner))
            {
                string storeOwner = (string)store_params[CommonStr.StoreParams.mainOwner];
                owners.Add(storeOwner);
            }
            
            if(store_params.ContainsKey(CommonStr.StoreParams.Owners))
            {
                owners = (List<string>)store_params[CommonStr.StoreParams.Owners];
            }

            managers = new List<string>();
            if (store_params.ContainsKey(CommonStr.StoreParams.Managers))
            {
                managers = (List<string>)store_params[CommonStr.StoreParams.Managers];
            }

            if (!store_params.ContainsKey(CommonStr.StoreParams.StoreInventory) || store_params[CommonStr.StoreParams.StoreInventory] == null)
            {
                Inventory = new Inventory();
            }
            else
            {
                Inventory = (Inventory)store_params[CommonStr.StoreParams.StoreInventory];
            }


            if (!store_params.ContainsKey(CommonStr.StoreParams.Validator) || store_params[CommonStr.StoreParams.Validator] == null)
            {
                PolicyValidator = new PolicyValidator(null, null);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.basketPriceAbove1000,
                  (PurchaseBasket basket, int productId) => basket.GetBasketOrigPrice() > 1000);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.Above1Unit,
                    (PurchaseBasket basket, int productId) => basket.Products.ContainsKey(productId) ? basket.Products[productId] > 1 : false);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.Above2Units,
                    (PurchaseBasket basket, int productId) => basket.Products.ContainsKey(productId) ? basket.Products[productId] > 2 : false);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.ProductPriceAbove100,
                    (PurchaseBasket basket, int productId) => basket.Products.ContainsKey(productId) ? basket.Store.GetProductDetails(productId).Item1.Price > 100 : false);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.ProductPriceAbove200,
                    (PurchaseBasket basket, int productId) => basket.Products.ContainsKey(productId) ? basket.Store.GetProductDetails(productId).Item1.Price > 200 : false);

                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.allwaysTrue,
                    (PurchaseBasket basket, int productId, string userName, int storeId) => true);

                PolicyValidator.AddDiscountFunction(CommonStr.DiscountPreConditions.NoDiscount,
                    (PurchaseBasket basket, int productId) => true);


                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.allwaysTrue,
                     (PurchaseBasket basket, int productId, string userName, int storeId) => true);

                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.singleOfProductType,
                    (PurchaseBasket basket, int productId, string userName, int storeId) => !basket.Products.ContainsKey(productId) ? true : basket.Products[productId] <= 1);

                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.Max10ProductPerBasket,
                    (PurchaseBasket basket, int productId, string userName, int storeId) => basket.GetNumProductsAtBasket() <= 10);

                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.GuestCantBuy,
                    (PurchaseBasket basket, int productId, string userName, int storeId) => UserManager.Instance.GetUser(userName) is null? false: UserManager.Instance.GetUser(userName).isguest());

                PolicyValidator.AddPurachseFunction(CommonStr.PurchasePreCondition.StoreMustBeActive,
                    (PurchaseBasket basket, int productId, string userName, int storeId) => StoreManagment.Instance.getStore(storeId) is null? false : StoreManagment.Instance.getStore(storeId).ActiveStore);


            }
            else
            {
                PolicyValidator = (PolicyValidator)store_params[CommonStr.StoreParams.Validator];
            }
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StoreDiscountPolicy) || store_params[CommonStr.StoreParams.StoreDiscountPolicy] == null)
            {
                DiscountPolicy = new ConditionalBasketDiscount(new DiscountPreCondition(CommonStr.DiscountPreConditions.NoDiscount), 0);
            }
            else
            {
                DiscountPolicy = (DiscountPolicy)store_params[CommonStr.StoreParams.StoreDiscountPolicy];
            }
            
            if(!store_params.ContainsKey(CommonStr.StoreParams.StorePuarchsePolicy) || store_params[CommonStr.StoreParams.StorePuarchsePolicy] == null)
            {
                PurchasePolicy = new BasketPurchasePolicy(new PurchasePreCondition(CommonStr.PurchasePreCondition.allwaysTrue));
            }
            else
            {
                PurchasePolicy = (PurchasePolicy)store_params[CommonStr.StoreParams.StorePuarchsePolicy];
            }

            if (!store_params.ContainsKey(CommonStr.StoreParams.StoreRank) || store_params[CommonStr.StoreParams.StoreRank] == null)
            {
                Rank = 1;
            }
            else
            {
                Rank = (int)store_params[CommonStr.StoreParams.StoreRank];
            }

            ActiveStore = true;
        }

  
        public Tuple<bool, string> IncreaseProductAmount(User user, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg + " user: " + user.getUserName().ToString() + "store: " + this.Id);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.IncreaseProductAmount(productId, amount, this.Id);
        }
        public List<string> getOwners()
        {
            return owners;
        }
        public Dictionary<string, string> getStaff()
        {
            Dictionary<string, string> staff = new Dictionary<string, string>();
            foreach(string owner in owners)
            {
                staff.Add(owner, CommonStr.StoreRoles.Owner);
            }
            foreach(string manager in managers)
            {
                staff.Add(manager, CommonStr.StoreRoles.Manager);
            }

            return staff;
        }

        public Tuple<bool, string> decrasePrdouctAmount(User user, int productId, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.DecraseProductAmount(productId, amount, this.Id);
        }

        public Tuple<bool,string> changeStoreStatus(User user, bool newStatus)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name))
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
            //DB-UPDATE
            DbManager.Instance.UpdateStore(DbManager.Instance.GetDbStore(this.Id), this);
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> UpdateDiscountPolicy(User user, DiscountPolicyData discountPolicyData)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.DiscountPolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }
            DiscountPolicy newPolicy = ToFatDiscountPolicy(discountPolicyData);
            if (newPolicy == null)
            {
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.PurchasePolicyErrMessage);

            }
            else
            {
                this.DiscountPolicy = newPolicy;

                //DB update Discount Policy
                DbManager.Instance.UpdateDiscountPolicy(newPolicy, this);
                return new Tuple<bool, string>(true, "");
            }
        }

        public Tuple<bool, string> UpdatePurchasePolicy(User user, PurchasePolicyData purchasePolicyData)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.PurachsePolicy))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }
            PurchasePolicy newPolicy = ToFatPurchasePolicy(purchasePolicyData);
            if (newPolicy == null) {
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.PurchasePolicyErrMessage);

            }
            else {
                this.PurchasePolicy = newPolicy;
                //DB update Purchase Policy
                DbManager.Instance.UpdatePurchasePolicy(newPolicy, this);
                return new Tuple<bool, string>(true, "");
            }
          
        }



        private PurchasePolicy ToFatPurchasePolicy(PurchasePolicyData thinPurchasePolicy)
        {
            if (thinPurchasePolicy.GetType() == typeof(PurchasePolicyProductData))
            {
                int policyProdutId = ((PurchasePolicyProductData)thinPurchasePolicy).ProductId;
                int preCondition = ((PurchasePolicyProductData)thinPurchasePolicy).PreCondition;
                return new ProductPurchasePolicy(new PurchasePreCondition(preCondition), policyProdutId);
            }

            else if (thinPurchasePolicy.GetType() == typeof(PurchasePolicyBasketData))
            {
                int preCondition = ((PurchasePolicyBasketData)thinPurchasePolicy).PreCondition;
                return new BasketPurchasePolicy(new PurchasePreCondition(preCondition));
            }

            else if (thinPurchasePolicy.GetType() == typeof(PurchasePolicySystemData))
            {
                int preCondition = ((PurchasePolicySystemData)thinPurchasePolicy).PreCondition;
                return new SystemPurchasePolicy(new PurchasePreCondition(preCondition), Id);
            }

            else if (thinPurchasePolicy.GetType() == typeof(PurchasePolicyUserData))
            {
                int preCondition = ((PurchasePolicyUserData)thinPurchasePolicy).PreCondition;
                string userName = ((PurchasePolicyUserData)thinPurchasePolicy).UserName;
                return new UserPurchasePolicy(new PurchasePreCondition(preCondition), userName);
            }

            else if (thinPurchasePolicy.GetType() == typeof(CompoundPurchasePolicyData))
            {
                int mergetype = ((CompoundPurchasePolicyData)thinPurchasePolicy).MergeType;
                List<PurchasePolicyData> policies = ((CompoundPurchasePolicyData)thinPurchasePolicy).PurchaseChildren;
                List<PurchasePolicy> retList = new List<PurchasePolicy>();
                foreach (PurchasePolicyData policy in policies)
                {
                    PurchasePolicy newPolicyData = ToFatPurchasePolicy(policy);
                    retList.Add(newPolicyData);
                }
                return new CompundPurchasePolicy(mergetype, retList);
            }
            return null; // not reached
        }

        private DiscountPolicy ToFatDiscountPolicy(DiscountPolicyData thinDiscountPolicy)
        {
            if (thinDiscountPolicy.GetType() == typeof(DiscountConditionalProductData))
            {
                int discountProdutId = ((DiscountConditionalProductData)thinDiscountPolicy).ProductId;
                int preCondition = ((DiscountConditionalProductData)thinDiscountPolicy).PreCondition;
                double discountPrecentage = ((DiscountConditionalProductData)thinDiscountPolicy).DiscountPercent;
                return new ConditionalProductDiscount(discountProdutId, new DiscountPreCondition(preCondition), discountPrecentage);
            }

            else if (DiscountPolicy.GetType() == typeof(DiscountConditionalBasketData))
            {
                int preCondition = ((DiscountConditionalBasketData)thinDiscountPolicy).PreCondition;
                double discountPrecentage = ((DiscountConditionalBasketData)thinDiscountPolicy).DiscountPercent;
                return new ConditionalBasketDiscount(new DiscountPreCondition(preCondition), discountPrecentage);
            }

            else if (DiscountPolicy.GetType() == typeof(DiscountRevealdData))
            {
                int discountProdutId = ((DiscountRevealdData)thinDiscountPolicy).ProductId;
                double discountPrecentage = ((DiscountRevealdData)thinDiscountPolicy).DiscountPrecent;
                return new RevealdDiscount(discountProdutId, discountPrecentage);
            }

            else if (DiscountPolicy.GetType() == typeof(CompoundDiscountPolicyData))
            {
                int mergetype = ((CompoundDiscountPolicyData)thinDiscountPolicy).MergeType;
                List<DiscountPolicyData> policies = ((CompoundDiscountPolicyData)thinDiscountPolicy).DiscountChildren;
                List<DiscountPolicy> retList = new List<DiscountPolicy>();
                foreach (DiscountPolicyData policy in policies)
                {
                    DiscountPolicy newPolicy = ToFatDiscountPolicy(policy);
                    retList.Add(newPolicy);
                }
                return new CompundDiscount(mergetype, retList);
            }
            return  null; // not reached
        }



        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.removeProduct(productId, this.Id);
        }

        public Tuple<bool, string> appendProduct(User user, Dictionary<string, object> productParams, int amount)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);

                }
            }

            return Inventory.appendProduct(productParams, amount, this.Id);
        }



        public Tuple<bool, string> UpdateProduct(User user, Dictionary<string, object> productParams)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (!owners.Contains(user.Name) && !managers.Contains(user.Name))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.notAOwnerOrManagerErrMsg);
            }

            if (managers.Contains(user.Name))
            {
                if (!user.getUserPermission(Id, CommonStr.MangerPermission.Product))
                {
                    Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                   
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.ManagerNoPermissionErrMsg);
                }
            }

            return Inventory.UpdateProduct(productParams);
        }

        public Tuple<bool, string> DecraseProductAmountAfterPuarchse(int productId, int amount)
        {
            return Inventory.DecraseProductAmount(productId, amount, this.Id);
        }
        public Tuple<bool, string> IncreaseProductAmountAfterFailedPuarchse(int productId, int amount)
        {
            return Inventory.IncreaseProductAmount(productId, amount, this.Id);
        }

        public Dictionary<string, object> getSotoreInfo()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            Dictionary<string, object> store_info = new Dictionary<string, object>();
            store_info.Add(CommonStr.StoreParams.StoreId, Id);
            if (owners.Count > 0)
                store_info.Add(CommonStr.StoreParams.mainOwner,owners[0]);
            store_info.Add(CommonStr.StoreParams.StoreInventory, Inventory);
            store_info.Add(CommonStr.StoreParams.StoreDiscountPolicy, DiscountPolicy);
            store_info.Add(CommonStr.StoreParams.StorePuarchsePolicy, PurchasePolicy);
            store_info.Add(CommonStr.StoreParams.IsActiveStore, ActiveStore);
            store_info.Add(CommonStr.StoreParams.StoreRank, Rank);
            return store_info;
        }

        public bool AddStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners.Contains(user.Name))
                return false;
            owners.Add(user.Name);
            return true;
        }

        public bool AddStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (managers.Contains(user.Name))
                return false;
            managers.Add(user.Name);
            return true;
        }

        public bool IsStoreOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return owners.Contains(user.Name);
        }
        
        public bool IsStoreManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return managers.Contains(user.Name);
        }

        public bool RemoveManager(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return managers.Remove(user.Name);
        }

        public bool RemoveOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return owners.Remove(user.Name);
        }


        internal int GetStoreId()
        {
            return Id;
        }


  


        //return product and its amount in the inventory
        public Tuple<Product, int> GetProductDetails(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return Inventory.getProductDetails(productId);
        }

        public bool ProductExist(int productId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            return Inventory.productExist(productId);
        }


        public double GetBasketOrigPrice(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Inventory.getBasketPrice(basket.products);
        }

        public double GetBasketPriceWithDiscount(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            double basketPrice = GetBasketOrigPrice(basket);
            double overallDiscount = DiscountPolicy.CalcDiscount(basket, PolicyValidator);
            double priceAfterDiscount = basketPrice - overallDiscount;

            return priceAfterDiscount;
        }

        public Tuple<bool, string> CheckIsValidBasket(PurchaseBasket basket)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            //checks if the basket accomodate the store's purchase policy
            if (!PurchasePolicy.IsEligiblePurchase(basket, PolicyValidator))
            {
                Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod(), CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.BasketNotAcceptPurchasePolicy);
            }
            return Inventory.isValidBasket(basket.Products);
        }

        public bool IsMainOwner(User user)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());

            if (owners[0] == user.Name)
                return true;
            else
                return false;
        }

        public string GetName()
        {
            return this.StoreName;
        }
    }
}
