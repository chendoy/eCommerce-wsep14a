﻿using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.DAL.StoreDb;
using Server.DAL.UserDb;
using Server.StoreComponent.DomainLayer;
using Server.UserComponent.Communication;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL
{
    public class DbManager
    {

        private EcommerceContext dbConn;
        private static readonly object padlock = new object();
        private static DbManager instance = null;

        public int GetNextProductId()
        {
            return dbConn.Products.Max(product => product.Id) + 1;
        }

        StoreAdapter storeAdpter;

        public static DbManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DbManager();
                        }
                    }
                }
                return instance;
            }
        }

        private DbManager()
        {
            dbConn = new EcommerceContext();
            storeAdpter = new StoreAdapter();
        }

        public NotifyData GetNotifyWithMaxId()
        {
           
            /*if (!dbConn.Notifies.Any())
            {
                // The table is empty
                return null;
            }
            return dbConn.Notifies.OrderByDescending(n => n.Id).FirstOrDefault();*/
            return null;
        }

        public List<User> GetAllUsers()
        {
            List<string> usernames = dbConn.Users.Select(user => user.Name).Distinct().ToList();
            List<User> users = new List<User>();
            foreach (string user in usernames)
            {
                users.Add(BuildUser(user));
            }
            return users;
        }

        public User BuildUser(string userName)
        {
            DbUser dbUser = dbConn.Users.Where(dbuser => dbuser.Name == userName).FirstOrDefault();
            User user = new User(1, dbUser.Name, dbUser.IsGuest, dbUser.IsAdmin);
            user.IsLoggedIn = dbUser.IsLoggedIn;

           /* Dictionary<int, Store> Store_Ownership = GetStoreOwnershipUser(userName);*/ //TODO
        //public LinkedList<NotifyData> unreadMessages { set; get; }
        //public Dictionary<int, Store> Store_Managment { get; }
        //public Dictionary<int, int[]> Store_options { get; }
        ////Contains the list of who appointed you to which store! not who you appointed to which store!
        //public Dictionary<int, User> AppointedByOwner { get; }
        //public Dictionary<int, User> AppointedByManager { get; }
        ////Version 3 Use case - 4.3 Addings.
        //public Dictionary<int, User> MasterAppointer { get; }
        ////Contains the list of who need to Approve his Ownership
        //public Dictionary<int, List<string>> NeedToApprove { get; }
        ////Contains the list of who need to Approve his Ownership
        //public Dictionary<int, List<string>> WaitingForApproval { get; }
        ////Contains the status of the Appoitment
        //public Dictionary<int, bool> IsApproved { get; }

            return user;
        }

        //private Dictionary<int, int> GetStoreOwnershipUser(string userName)
        //{

        //    List<StoreOwnershipAppoint> storeOwnershipAppoints = dbConn.StoreOwnershipAppoints.Where(soa => soa.AppointerName == userName).ToList();
        //    Dictionary<int, int> ownershipsDict = new Dictionary<int, int>();  
        //    foreach(StoreOwnershipAppoint soa in storeOwnershipAppoints)
        //    {
        //        ownershipsDict.Add()
        //    }
        //}

       public void InsertStore(Store store)
        {
            DbStore dbStore = new DbStore(store.Id, store.Rank, store.StoreName, store.ActiveStore);
            dbConn.Stores.Add(dbStore);
            //InsertInventory(store.Inventory, store.Id);  TODO: impl!
            InsertDiscountPolicy(store.DiscountPolicy, store.Id, parentId: null);
            InsertPurchasePolicy(store.PurchasePolicy, store.Id, parentId: null);
            InsertOwners(store.owners, store.Id);
            InsertManagers(store.managers, store.Id);
        }

        private void InsertPurchasePolicy(PurchasePolicy policyData, int storeId, int? parentId)
        {
            if (policyData.GetType() == typeof(ProductPurchasePolicy))
            {
                int policyProdutId = ((ProductPurchasePolicy)policyData).policyProductId;
                int preCondition = ((ProductPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: policyProdutId,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.ProductPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(BasketPurchasePolicy))
            {
                int preCondition = ((BasketPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.BasketPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(SystemPurchasePolicy))
            {

                int preCondition = ((SystemPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition);
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                 mergetype: null,
                                                                 parentid: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 policyproductid: null,
                                                                 buyerusername: null,
                                                                 purchasepolictype: CommonStr.PurchasePolicyTypes.SystemPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(UserPurchasePolicy))
            {
                string username = ((UserPurchasePolicy)policyData).UserName;
                int preCondition = ((UserPurchasePolicy)policyData).PreCondition.PreConditionNumber;
                dbConn.PurchasePolicies.Add(new DbPurchasePolicy(storeId: storeId,
                                                                mergetype: null,
                                                                parentid: parentId,
                                                                preconditionid: null,
                                                                policyproductid: null,
                                                                buyerusername: username,
                                                                purchasepolictype: CommonStr.PurchasePolicyTypes.UserPurchasePolicy));
                dbConn.SaveChanges();
            }

            else if (policyData.GetType() == typeof(CompundPurchasePolicy))
            {
                int mergetype = ((CompundPurchasePolicy)policyData).mergeType;
                DbPurchasePolicy dbPurchase = new DbPurchasePolicy(storeId: storeId,
                                                                   mergetype: mergetype,
                                                                   parentid: parentId,
                                                                   preconditionid: null,
                                                                   policyproductid: null,
                                                                   buyerusername: null,
                                                                   purchasepolictype: CommonStr.PurchasePolicyTypes.CompundPurchasePolicy);
                dbConn.PurchasePolicies.Add(dbPurchase);
                dbConn.SaveChanges();

                int dbPolicyId = GetDbPurchsePolicyId(dbPurchase, storeId, null);
                List<PurchasePolicy> policies = ((CompundPurchasePolicy)policyData).getChildren();

                foreach (PurchasePolicy policy in policies)
                {
                    InsertPurchasePolicy(policy, storeId, dbPolicyId);
                }

            }

        }
        private void InsertDiscountPolicy(DiscountPolicy discountPolicy, int storeId, int? parentId)
        {

            if (discountPolicy.GetType() == typeof(ConditionalProductDiscount))
            {
                int discountProdutId = ((ConditionalProductDiscount)discountPolicy).discountProdutId;
                int preCondition_num = ((ConditionalProductDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalProductDiscount)discountPolicy).Discount;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition_num);
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalProductDiscount));
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(ConditionalBasketDiscount))
            {
                int preCondition = ((ConditionalBasketDiscount)discountPolicy).PreCondition.PreConditionNumber;
                double discountPrecentage = ((ConditionalBasketDiscount)discountPolicy).Discount;
                DbPreCondition dbPreCondition = GetDbPreCondition(preCondition);
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: dbPreCondition.Id,
                                                                 discountproductid: null,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.ConditionalBasketDiscount));
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(RevealdDiscount))
            {
                int discountProdutId = ((RevealdDiscount)discountPolicy).discountProdutId;
                double discountPrecentage = ((RevealdDiscount)discountPolicy).discount;
                dbConn.DiscountPolicies.Add(new DbDiscountPolicy(storeid: storeId,
                                                                 mergetype: null,
                                                                 parentId: parentId,
                                                                 preconditionid: null,
                                                                 discountproductid: discountProdutId,
                                                                 discount: discountPrecentage,
                                                                 discounttype: CommonStr.DiscountPolicyTypes.RevealdDiscount));
                dbConn.SaveChanges();
            }

            else if (discountPolicy.GetType() == typeof(CompundDiscount))
            {
                int mergetype = ((CompundDiscount)discountPolicy).GetMergeType();
                DbDiscountPolicy dbDiscount = new DbDiscountPolicy(storeid: storeId,
                                                              mergetype: mergetype,
                                                              parentId: parentId,
                                                              preconditionid: null,
                                                              discountproductid: null,
                                                              discount: null,
                                                              discounttype: CommonStr.DiscountPolicyTypes.CompundDiscount));
                dbConn.DiscountPolicies.Add(dbDiscount);
                dbConn.SaveChanges();
                int dbPolicyId = GetDbDiscountPolicyId(dbDiscount, storeId, null);
                List<DiscountPolicy> policies = ((CompundDiscount)discountPolicy).getChildren();
                foreach (DiscountPolicy policy in policies)
                {
                    InsertDiscountPolicy(discountPolicy: policy, storeId: storeId, parentId: dbPolicyId);
                }
            }
        }

        private int GetDbDiscountPolicyId(DbDiscountPolicy dbDiscountPolicy, int storeId, int? precondition)
        {
            DbDiscountPolicy dbFromDb = dbConn.DiscountPolicies.Where(dbDiscount => dbDiscountPolicy.Discount == dbDiscount.Discount &
                                                        dbDiscountPolicy.DiscountProductId == dbDiscount.DiscountProductId &
                                                        dbDiscountPolicy.MergeType == dbDiscount.MergeType &
                                                        dbDiscountPolicy.StoreId == storeId &
                                                        dbDiscountPolicy.ParentId == dbDiscount.ParentId &
                                                        GetDbPreConditionNumberById((int)dbDiscountPolicy.PreConditionId) == precondition).FirstOrDefault();
            return dbFromDb.Id;
                                                        
        }


        private int GetDbPurchsePolicyId(DbPurchasePolicy dbPurchasePolicy, int storeId, int? precondition)
        {
            DbPurchasePolicy dbFromDb = dbConn.PurchasePolicies.Where(dbPurchase => dbPurchasePolicy.BuyerUserName == dbPurchase.BuyerUserName &
                                                                    dbPurchasePolicy.MergeType == dbPurchase.MergeType&
                                                                    dbPurchasePolicy.PolicyProductId == dbPurchase.PolicyProductId &
                                                                    dbPurchasePolicy.StoreId == storeId &
                                                                    dbPurchasePolicy.ParentId == dbPurchase.ParentId &
                                                                    GetDbPreConditionNumberById((int)dbPurchasePolicy.PreConditionId) == precondition
                                                                    ).FirstOrDefault();
            return dbFromDb.Id;

        }

        private int GetDbPreConditionNumberById(int Id)
        {
            return dbConn.PreConditions.Where(pre => pre.Id == Id).FirstOrDefault().PreConditionNum;
        }

        private DbPreCondition GetDbPreCondition(int preCondition_num)
        {
            return dbConn.PreConditions.Where(pre => pre.PreConditionNum == preCondition_num).FirstOrDefault();
        }

        private void InsertPurchasePolicy(PurchasePolicy purchasepolicy)
        {

        }

        private void InsertOwners(List<string> owners, int storeId)
        {
            foreach(string owner in owners)
            {
                InsertOwner(owner, storeId);
            }
            dbConn.SaveChanges();
        }

        private void InsertManagers(List<string> managers, int storeId)
        {
            foreach (string manager in managers)
            {
                InsertManager(manager, storeId);
            }
            dbConn.SaveChanges();
        }

        private void InsertOwner(string ownerName, int storeId)
        {
            dbConn.StoreManagers.Add(new StoreManager(storeId, ownerName));

        }

        private void InsertManager(string managerName, int storeId)
        {
            dbConn.StoreManagers.Add(new StoreManager(storeId, managerName));

        }

        //private void InsertInventory(Inventory inventory, int StoreId)
        //{
        //    List<DbInventoryItem> inv_items = new List<DbInventoryItem>();
        //    foreach (KeyValuePair<int, Tuple<Product, int>> entry in inventory.InvProducts)
        //    {
        //        inv_items.Add(new DbInventoryItem(StoreId, ent))
        //    }
        //}

        public List<Store> GetAllStores()
        {
            List<int> storesIds = dbConn.Stores.Select(store => store.Id).Distinct().ToList();
            List<Store> stores = new List<Store>();
            foreach(int storeId in storesIds)
            {
                stores.Add(GetStore(storeId));
            }
            return stores;
        }
        private Store GetStore(int StoreId)
        {
            DbStore dbstore = dbConn.Stores.Where(store => store.Id == StoreId).FirstOrDefault();
            DiscountPolicy discountPolicy = GetDiscountPolicy(StoreId);
            PurchasePolicy purchasePolicy = GetPurchasePolicy(StoreId);
            Inventory inventory = GetInventory(StoreId);
            Dictionary<string, object> store_params = new Dictionary<string, object>();
            store_params.Add(CommonStr.StoreParams.StoreId, StoreId);
            store_params.Add(CommonStr.StoreParams.StoreName, dbstore.StoreName);
            store_params.Add(CommonStr.StoreParams.IsActiveStore, dbstore.ActiveStore);
            store_params.Add(CommonStr.StoreParams.StoreRank, dbstore.Rank);
            store_params.Add(CommonStr.StoreParams.StorePuarchsePolicy, purchasePolicy);
            store_params.Add(CommonStr.StoreParams.StoreDiscountPolicy, discountPolicy);
            store_params.Add(CommonStr.StoreParams.StoreInventory, inventory);
            List<string> owners = GetStoreOwners(StoreId);
            store_params.Add(CommonStr.StoreParams.Owners, owners);
            List<string> managers= GetStoreManagers(StoreId);
            store_params.Add(CommonStr.StoreParams.Managers, GetStoreManagers(StoreId));
            return new Store(store_params);
        }

        private DiscountPolicy GetDiscountPolicy(int storeId)
        {
            List<DbDiscountPolicy> storeDiscountPolicies = dbConn.DiscountPolicies.Where(policy => policy.StoreId == storeId).ToList();
            return storeAdpter.ComposeDiscountPolicy(storeDiscountPolicies);
        }

        private Inventory GetInventory(int StoreId)
        {
            List<DbInventoryItem> invItems = dbConn.InventoriesItmes.Where(item => item.StoreId == StoreId).ToList();
            if(invItems.Count == 0)
            {
                return new Inventory();
            }
            else
            {
                Dictionary<int, Tuple<Product, int>> inventoryProducts = new Dictionary<int, Tuple<Product, int>>();
                foreach (DbInventoryItem item in invItems)
                {
                    DbProduct dbProduct = dbConn.Products.Where(prod => prod.Id == item.ProductId).FirstOrDefault();
                    Product product = new Product(dbProduct.Id, dbProduct.Details, dbProduct.Price, dbProduct.Name, dbProduct.Rank, dbProduct.Category, dbProduct.ImgUrl);
                    inventoryProducts.Add(item.ProductId, new Tuple<Product, int>(product, item.Amount));
                }
                return new Inventory(inventoryProducts);
            }
          
            
        }

        private List<string> GetStoreOwners(int StoreId)
        {
            List<StoreOwner> owners = dbConn.StoreOwners.Where(owner => owner.StoreId == StoreId).ToList();
            if(owners.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                List<string> userOwners = new List<string>();
                foreach (StoreOwner owner in owners)
                {
                    userOwners.Add(owner.OwnerName);
                }
                return userOwners;
            }
        }

        private List<string> GetStoreManagers(int StoreId)
        {
            List<StoreManager> managers = dbConn.StoreManagers.Where(manager => manager.StoreId == StoreId).ToList();
            if(managers.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                List<string> userManagers = new List<string>();
                foreach (StoreManager manager in managers)
                {
                    userManagers.Add(manager.ManagerName);
                }
                return userManagers;
            }
     
        }

        public PreCondition GetPreCondition(int PreConditionId)
        {
            DbPreCondition pre = dbConn.PreConditions.Where(precon => precon.Id == PreConditionId).FirstOrDefault();
            if(pre.PreConditionType == CommonStr.PreConditionType.DiscountPreCondition)
            {
                return new DiscountPreCondition(pre.PreConditionNum);
            }
            else
            {
                return new PurchasePreCondition(pre.PreConditionNum);
            }
        }

        private PurchasePolicy GetPurchasePolicy(int storeId)
        {
            List<DbPurchasePolicy> storePurchasePolicies = dbConn.PurchasePolicies.Where(policy => policy.StoreId == storeId).ToList();
            return storeAdpter.ComposePurchasePolicy(storePurchasePolicies);
        }
        public void InsertUserNotification(NotifyData notification)
        {
            //dbConn.Notifies.Add(notification);
            //dbConn.SaveChanges();
        }

        public  DbDiscountPolicy getParentDiscount(DbDiscountPolicy dbDiscountPolicy)
        {
            return dbConn.DiscountPolicies.Where(policy => policy.Id == dbDiscountPolicy.ParentId).FirstOrDefault();
        }

        public DbPurchasePolicy getParentPurchasePolicy(DbPurchasePolicy dbPurchasePolicy)
        {
            return dbConn.PurchasePolicies.Where(policy => policy.Id == dbPurchasePolicy.ParentId).FirstOrDefault();
        }

   
    }
}
