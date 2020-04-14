using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce_14a
{
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;
        private UserManager userManager;
        string notExistOrActiveMessage = "user doesn't exist or not active";
        string storeAlreadyExistMessage = "store with this id already exist!";
        string storeNotExistMessage = "store Not Exists";
        string notMainOwnerErrMessage = "action cann't be performed because this user is not main owner";
        public StoreManagment(Dictionary<int, Store> stores, UserManager userManager)
        {
            this.stores = stores;
            this.userManager = userManager;
        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            if (!stores.ContainsKey(storeId))
                return new Dictionary<string, object>();
            return stores[storeId].getSotoreInfo();
        }

        public Tuple<bool, string> addProductAmount(int storeId, string userName, int productId, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            return stores[storeId].addProductAmount(user:user, productId: productId, amount: amount);
        }

        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            return stores[storeId].removeProduct(user, productId);
        }

        public Tuple<bool, string> appendProduct(int storeId, string userName, int pId, string pDetails, double pPrice, string pName, string pCategory, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);
            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add("Id", pId);
            productParams.Add("Details", pDetails);
            productParams.Add("Price", pPrice);
            productParams.Add("Name", pName);
            productParams.Add("Category", pCategory);
            return stores[storeId].appendProduct(user, productParams, amount);
        }

        public Store getStore(int storeId)
        {
            Store store;
            if (!stores.TryGetValue(storeId, out store))
                return null;
            return store;
        }

        public Tuple<bool, string> decraseProduct(int storeId, string userName, int productId, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);
            
            return stores[storeId].decrasePrdouct(user:user , productId: productId, amount: amount);
        }

        //should open store and add it to the current list
        public Tuple<int, string> createStore(string userName, int discountType, int puarchseType)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<int, string>(-1, notExistOrActiveMessage);

            int newStoreId = stores.Keys.Max() + 1;

            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            storeParam.Add("Id", newStoreId);
            storeParam.Add("Owner", user);
            storeParam.Add("DiscountPolicy", new DiscountPolicy(discountType));
            storeParam.Add("PuarchasePolicy", new PuarchsePolicy(puarchseType));
            Store store = new Store(storeParam);

            Tuple<bool, string> ownershipAdded = user.addStoreOwnership(store);
            if (!ownershipAdded.Item1)
                return new Tuple<int, string>(-1, ownershipAdded.Item2);

            stores.Add(newStoreId, store);

            return new Tuple<int, string>(newStoreId, "");
        }

        //talk with sundy on impl
        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!userManager.isMainOwner(user, storeId))
                return new Tuple<bool, string>(false, notMainOwnerErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            if (!user.isStoreOwner(storeId))
                return new Tuple<bool, string>(false, "user" + userName + " not store owner of " + storeId.ToString());

            Tuple<bool, string> ownerShipedRemoved = userManager.removeAllFromStore(storeId);
            if (!ownerShipedRemoved.Item1)
                return ownerShipedRemoved;

            stores.Remove(storeId);

            return new Tuple<bool, string>(true, "");
        }


        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            return stores[storeId].changeStoreStatus(user, status);

        }
        
        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, notExistOrActiveMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add("Id", productId);
            productParams.Add("Details", pDetails);
            productParams.Add("Price", pPrice);
            productParams.Add("Name", pName);
            productParams.Add("Category", pCategory);
            return stores[storeId].UpdateProduct(user, productParams);
        }
    


        public Dictionary<int, Store> getActiveSotres()
        {
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


        // return product amount, if there is exception the function returns -1
        public int getProductAmount(int storeId, int productId)
        {
            Tuple<Product, int> details = getProductDetails(storeId, productId);
            if (details != null)
                return details.Item2;
            else
                return -1;
        }

        //return Product object, on exception function will return null
        public Product getProduct(int storeId, int productId)
        {
            Tuple<Product, int> details = getProductDetails(storeId, productId);
            if (details != null)
                return details.Item1;
            else
                return null;
        }
         

        //return product and it's amount, on exception the function will return null
        private Tuple<Product, int> getProductDetails(int storeId, int productId)
        {
            if (!stores.ContainsKey(storeId))
                return null;
            else
                return stores[storeId].getProductDetails(productId);
        }



        public  bool CheckValidProduct(int storeId, int productId)
        {
            if (!stores.ContainsKey(storeId))
                return false;

           return  stores[storeId].productExist(productId);
        }

        // Gets Dictionary of Product ID and Amount
        public  Tuple<bool, string> CheckValidBasketForStore(int storeId, Dictionary<int, int> products)
        {
            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, storeNotExistMessage);

            return stores[storeId].checkIsValidBasket(products);
        }

        // Gets Dictionary of Product ID and Amount
        public double GetBasketPrice(int storeId, Dictionary<int, int> products)
        {
            if (!stores.ContainsKey(storeId))
                return -1;

            return stores[storeId].getBucketPrice(products);
        }
 

      

    }
}
