using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class StoresManagment
    {
        private Dictionary<int, Store> stores;
        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            if (!stores.ContainsKey(storeId))
                return new Dictionary<string, object>();
            return stores[storeId].getSotoreInfo();
        }

        public Tuple<bool, Exception> addProduct(int storeId, int userId, Product p, int amount)
        {
            Tuple<bool,Exception> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].addProductAmount(userId: userId, product: p, amount: amount);
        }

        public Tuple<bool, Exception> decraseProduct(int storeId, int userId, Product p, int amount)
        {
            Tuple<bool, Exception> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].decrasePrdouct(userId: userId, product: p, amount: amount);
        }

        public Tuple<bool, Exception> addSotre(Store store)
        {
            //whenever stores are created it should be added here
            Tuple<bool, Exception> storeNotExistRes = storeNotExist(store.Id);
            bool notExist = storeNotExistRes.Item1;
            if (!notExist)
            {
                return storeNotExistRes;
            }

            stores.Add(store.Id, store);
            return new Tuple<bool, Exception>(true, null);
        }

        public Tuple<bool, Exception> removeStore(int storeId)
        {
            //whenever stores are created it should be added here

            Tuple<bool, Exception> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }

            stores.Remove(storeId);
            return new Tuple<bool, Exception>(true, new Exception(""));
        }

        public Tuple<bool, Exception> changeStoreStatus(int storeId, bool status)
        {
            Tuple<bool, Exception> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].changeStoreStatus(status);

        }


        public Tuple<bool, Exception> UpdatePrdocutDetails(int storeId, int userId,int productId, string newDetails)
        {
            Tuple<bool, Exception> storeExistRes = storeExist(storeId);
            bool isExist = storeExistRes.Item1;
            if (!isExist)
            {
                return storeExistRes;
            }
            return stores[storeId].UpdatePrdocutDetails(userId:userId, productId:productId, newDetails: newDetails);

        }

        public List<Product> SearchProducts(Dictionary<string, object> searchBy)
        {
            List<Product> matchProducts = new List<Product>();
            foreach (KeyValuePair<int, Store> entry in stores)
            {
                Store store = entry.Value;
                Dictionary<string, object> storeInfo = store.getSotoreInfo();
                Inventory storeInv = (Inventory)storeInfo["inventory"];

                if (searchBy.ContainsKey("SearchByStoreRank"))
                {
                    int minRank = (int)searchBy["SearchByStoreRank"];
                    if (store.Rank < minRank)
                    {
                        continue;
                    }
                }

                foreach (KeyValuePair<int, Tuple<Product, int>> entryProduct in storeInv.Inv)
                {
                    Product product = entryProduct.Value.Item1;
                    if (searchBy.ContainsKey("searchByName"))
                    {
                        if (!product.Name.Equals(searchBy["searchByName"]))
                        {
                            continue;
                        }
                    }

                    if (searchBy.ContainsKey("searchByCategory"))
                    {

                        if (!product.Category.Equals(searchBy["searchByCategory"]))
                        {
                            continue;
                        }
                    }

                    if (searchBy.ContainsKey("searchByPriceRange"))
                    {
                        Tuple<double, double> priceRange = (Tuple<double, double>)searchBy["searchByPriceRange"];
                        double minPrice = priceRange.Item1;
                        double maxPrice = priceRange.Item2;
                        if (product.Price > maxPrice || product.Price < minPrice)
                        {
                            continue;
                        }

                    }
                    if (searchBy.ContainsKey("searchByProductRank"))
                    {
                        int minRank = (int)searchBy["searchByRank"];
                        if (product.Rank < minRank)
                        {
                            continue;
                        }
                    }

                    if (searchBy.ContainsKey("SearchByKeyWord"))
                    {
                        string keyWord = (string)searchBy["SearchByKeyWord"];
                        if (!product.Name.Contains(keyWord))
                        {
                            continue;
                        }
                    }
                    matchProducts.Add(product);
                }

            }
            return matchProducts;
        }


        private Tuple<bool, Exception> storeExist(int store_id)
        {
            if (stores.ContainsKey(store_id))
            {
                return new Tuple<bool, Exception>(false, new Exception("store not Exist!"));
            }
            else
            {
                return new Tuple<bool, Exception>(true, new Exception(""));
            }
        }

        private Tuple<bool, Exception> storeNotExist(int store_id)
        {
            if (stores.ContainsKey(store_id))
            {
                return new Tuple<bool, Exception>(false, new Exception("store with this id already exist!"));
            }
            else
            {
                return new Tuple<bool, Exception>(true, new Exception(""));
            }
        }

    }


}
