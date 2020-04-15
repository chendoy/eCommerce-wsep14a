using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Store
    {
        private DiscountPolicy discountPolicy;
        private PuarchsePolicy puarchsePolicy;
        private Inventory inventory;
        private int rank;

  
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
            this.ActiveStore = true;

            if (store_params.ContainsKey(CommonStr.StoreParams.StoreRank))
                this.rank = (int)store_params[CommonStr.StoreParams.StoreRank];
            else
                this.rank = 1;

        }

        public Tuple<bool, string> addProductAmount(User user, int productId, int amount)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);

            return inventory.addProductAmount(productId, amount);
        }

        public Tuple<bool, string> decrasePrdouct(User user, int productId, int amount)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);

            return inventory.DecraseProductAmount(productId, amount);
        }
        public Tuple<bool,string> changeStoreStatus(User user, bool newStatus)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);

            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);
                }
            }
            ActiveStore = newStatus;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);

            return inventory.removeProduct(productId);
        }

        public Tuple<bool, string> appendProduct(User user, Dictionary<string, object> productParams, int amount)
        {
            
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);
            return inventory.appendProduct(productParams, amount);
        }



        public Tuple<bool, string> UpdateProduct(User user, Dictionary<string, object> productParams)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, CommonStr.StoreErrorMessage.userIsNotOwnerErrMsg);

            return inventory.UpdateProduct(productParams);
        }

        public Dictionary<string, object> getSotoreInfo()
        {
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
            if (user.isguest() || owners.Contains(user))
                return false;
            owners.Add(user);
            return true;
        }

        public bool AddStoreManager(User user)
        {
            if (user.isguest() || managers.Contains(user))
                return false;
            managers.Add(user);
            return true;
        }

        public bool IsStoreOwner(User user)
        {
            return owners.Contains(user);
        }
        
        public bool IsStoreManager(User user)
        {
            return managers.Contains(user);
        }

        public bool RemoveManager(User user)
        {
            return managers.Remove(user);
        }

        public bool RemoveOwner(User user)
        {
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
     
        public bool ActiveStore { get; private set; }

        public Tuple<Product, int> getProductDetails(int productId)
        {
            return inventory.getProductDetails(productId);
        }

        public bool productExist(int productId)
        {
            return inventory.productExist(productId);
        }

        public double getBucketPrice(Dictionary<int, int> products)
        {
            return inventory.getBasketPrice(products);
        }

        public Tuple<bool, string> checkIsValidBasket(Dictionary<int, int> products)
        {
            return inventory.isValidBasket(products);
        }

        public bool isMainOwner(User user)
        {
            if (owners[0] == user)
                return true;
            else
                return false;
        }
    }
}
