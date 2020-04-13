using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text;

namespace eCommerce_14a
{
    public class Store
    {
        private DiscountPolicy discountPolicy;
        private PuarchsePolicy puarchsePolicy;
        string notOwnerMessage = "this user isn't a store owner, thus he can't update inventory products details";
        public Store(Dictionary<string, object> store_params)
        {
            this.Id = (int)store_params["Id"];
            this.owners = new List<User>();
            User storeOwner = (User)store_params["Owner"];
            this.owners.Add(storeOwner);
            this.managers = new List<User>();
            this.Inventory = new Inventory();
            this.discountPolicy = (DiscountPolicy)store_params["DiscountPolicy"];
            this.puarchsePolicy = (PuarchsePolicy)store_params["PuarchasePolicy"];
            this.ActiveStore = true;
            this.Rank = 3;
        }

        public Tuple<bool, string> addProductAmount(User user, int productId, int amount)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, notOwnerMessage);

            return Inventory.addProductAmount(productId, amount);
        }

        public Tuple<bool, string> decrasePrdouct(User user, int productId, int amount)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, notOwnerMessage);

            return Inventory.DecraseProductAmount(productId, amount);
        }
        public Tuple<bool,string> changeStoreStatus(bool newStatus)
        {
            if (newStatus)
            {
                if(owners.Count == 0)
                {
                    return new Tuple<bool, string>(false, "Cann't change store status to active until store has at least one owner");
                }
            }
            ActiveStore = newStatus;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(User user, int productId)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, notOwnerMessage);

            return Inventory.removeProduct(productId);
        }

        public Tuple<bool, string> appendProduct(User user, Dictionary<string, object> productParams, int amount)
        {
            
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, notOwnerMessage);
            return Inventory.appendProduct(productParams, amount);
        }



        public Tuple<bool, string> UpdateProduct(User user, Dictionary<string, object> productParams)
        {
            if (!owners.Contains(user))
                return new Tuple<bool, string>(false, notOwnerMessage);

            return Inventory.UpdateProduct(productParams);
        }

        public Dictionary<string, object> getSotoreInfo()
        {
            Dictionary<string, object> store_info = new Dictionary<string, object>();
            store_info.Add("id", Id);
            store_info.Add("owners", owners);
            store_info.Add("managers", managers);
            store_info.Add("inventory", Inventory);
            store_info.Add("discount_policy", discountPolicy);
            store_info.Add("puarchse_policy", puarchsePolicy);
            store_info.Add("is_active", ActiveStore);
            store_info.Add("rank", Rank);
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

        public int Rank { get; set; }

        public Inventory Inventory { get; }
        public bool ActiveStore { get; private set; }

    }
}
