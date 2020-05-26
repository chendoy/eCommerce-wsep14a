using eCommerce_14a.StoreComponent.DomainLayer;
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
    class DbManager
    {

        private EcommerceContext dbConn;
        private static readonly object padlock = new object();
        private static DbManager instance = null;
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
            store_params.Add(CommonStr.StoreParams.Owners, GetStoreOwners(StoreId));
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
            Dictionary<int, Tuple<Product, int>> inventoryProducts = new Dictionary<int, Tuple<Product, int>>();
            foreach(DbInventoryItem item in invItems)
            {
                DbProduct dbProduct = dbConn.Products.Where(prod => prod.Id == item.ProductId).FirstOrDefault();
                Product product = new Product(dbProduct.Id, dbProduct.Details, dbProduct.Price, dbProduct.Name, dbProduct.Rank, dbProduct.Category, dbProduct.ImgUrl);
                inventoryProducts.Add(item.ProductId, new Tuple<Product, int>(product, item.Amount));
            }
            return new Inventory(inventoryProducts);
            
        }

        private List<string> GetStoreOwners(int StoreId)
        {
            List<StoreOwner> owners = dbConn.StoreOwners.Where(owner => owner.StoreId == StoreId).ToList();
            List<string> userOwners = new List<string>();
            foreach(StoreOwner owner in owners)
            {
                userOwners.Add(owner.OwnerName);
            }
            return userOwners;
        }

        private List<string> GetStoreManagers(int StoreId)
        {
            List<StoreManager> managers = dbConn.StoreManagers.Where(manager => manager.StoreId == StoreId).ToList();
            List<string> userManagers = new List<string>();
            foreach (StoreManager manager in managers)
            {
                userManagers.Add(manager.ManagerName);
            }
            return userManagers;
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
