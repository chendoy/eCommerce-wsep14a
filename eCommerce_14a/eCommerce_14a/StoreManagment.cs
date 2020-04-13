using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;

        public StoreManagment(Dictionary<int, Store> stores)
        {
            this.stores = stores;
        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            if (!stores.ContainsKey(storeId))
                return new Dictionary<string, object>();
            return stores[storeId].getSotoreInfo();
        }

        public Tuple<bool, string> addProduct(int storeId, int userId, Product p, int amount)
        {
            Tuple<bool, string> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].addProductAmount(userId: userId, product: p, amount: amount);
        }

        public Tuple<bool, string> decraseProduct(int storeId, int userId, Product p, int amount)
        {
            Tuple<bool, string> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].decrasePrdouct(userId: userId, product: p, amount: amount);
        }


        //talk with sundy on impl
        //should open store and add it to the current list
        public Tuple<bool, string> createStore(int userId, int storeId)
        {
            throw new NotImplementedException();
        }

        //talk with sundy on impl
        public Tuple<bool, string> removeStore(int userId, int storeId)
        {
            throw new NotImplementedException();
        }

        //public Tuple<bool, string> addSotre(Store store)
        //{
        //    //whenever stores are created it should be added here
        //    Tuple<bool, string> storeNotExistRes = storeNotExist(store.Id);
        //    bool notExist = storeNotExistRes.Item1;
        //    if (!notExist)
        //    {
        //        return storeNotExistRes;
        //    }

        //    stores.Add(store.Id, store);
        //    return new Tuple<bool, string>(true, "");
        //}



        //public Tuple<bool, string> removeStore(int storeId)
        //{
        //    //whenever stores are created it should be added here

        //    Tuple<bool, string> storeExistRes = storeExist(storeId);
        //    bool isExist = storeExistRes.Item1;
        //    if (!isExist)
        //    {
        //        return storeExistRes;
        //    }

        //    stores.Remove(storeId);
        //    return new Tuple<bool, string>(true, "");
        //}

        public Tuple<bool, string> changeStoreStatus(int storeId, bool status)
        {
            Tuple<bool, string> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].changeStoreStatus(status);

        }


        public Tuple<bool, string> UpdatePrdocutDetails(int storeId, int userId, int productId, string newDetails)
        {
            Tuple<bool, string> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].UpdatePrdocutDetails(userId: userId, productId: productId, newDetails: newDetails);

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

        private Tuple<bool, string> storeExist(int store_id)
        {
            if (stores.ContainsKey(store_id))
            {
                return new Tuple<bool, string>(false, "store not Exist!");
            }
            else
            {
                return new Tuple<bool, string>(true, "");
            }
        }

        private Tuple<bool, string> storeNotExist(int store_id)
        {
            if (stores.ContainsKey(store_id))
            {
                return new Tuple<bool, string>(false, "store with this id already exist!");
            }
            else
            {
                return new Tuple<bool, string>(true, "");
            }
        }

    }
}
