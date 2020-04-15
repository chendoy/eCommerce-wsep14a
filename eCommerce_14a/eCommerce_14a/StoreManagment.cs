using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce_14a
{
    public class StoreManagment
    {
        private Dictionary<int, Store> stores;
        private int nextStoreId = 0;
        private UserManager userManager;

        public StoreManagment(Dictionary<int, Store> stores)
        {
            userManager = UserManager.Instance;
            if(stores is null)
            {
                this.stores = new Dictionary<int, Store>();
            }
            this.stores = stores;
            this.userManager = UserManager.Instance;
        }

      

        public Tuple<bool, string> appendProduct(int storeId, string userName, int pId, string pDetails, double pPrice, string pName, string pCategory, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);
            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductId, pId);
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            return stores[storeId].appendProduct(user, productParams, amount);
        }

        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {

            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            return stores[storeId].removeProduct(user, productId);
        }



        public Tuple<bool, string> addProductAmount(int storeId, string userName, int productId, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            return stores[storeId].addProductAmount(user: user, productId: productId, amount: amount);
        }

        public Tuple<bool, string> decraseProduct(int storeId, string userName, int productId, int amount)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);
            
            return stores[storeId].decrasePrdouct(user:user , productId: productId, amount: amount);
        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            if (!stores.ContainsKey(storeId))
                return new Dictionary<string, object>();
            return stores[storeId].getSotoreInfo();
        }


     
        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            return stores[storeId].changeStoreStatus(user, status);

        }
        
        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            Dictionary<string, object> productParams = new Dictionary<string, object>();
            productParams.Add(CommonStr.ProductParams.ProductId, productId);
            productParams.Add(CommonStr.ProductParams.ProductDetails, pDetails);
            productParams.Add(CommonStr.ProductParams.ProductPrice, pPrice);
            productParams.Add(CommonStr.ProductParams.ProductName, pName);
            productParams.Add(CommonStr.ProductParams.ProductCategory, pCategory);
            return stores[storeId].UpdateProduct(user, productParams);
        }


        public Store getStore(int storeId)
        {
            if (stores.ContainsKey(storeId))
                return stores[storeId];
            return null;
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
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);

            return stores[storeId].checkIsValidBasket(products);
        }

        // Gets Dictionary of Product ID and Amount
        public double GetBasketPrice(int storeId, Dictionary<int, int> products)
        {
            if (!stores.ContainsKey(storeId))
                return -1;

            return stores[storeId].getBucketPrice(products);
        }


        //impl on next version only!
        public Tuple<int, string> createStore(string userName, int discountType, int puarchseType)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<int, string>(-1, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            nextStoreId += 1;

            Dictionary<string, object> storeParam = new Dictionary<string, object>();
            storeParam.Add(CommonStr.StoreParams.StoreId, nextStoreId);
            storeParam.Add(CommonStr.StoreParams.mainOwner, user);
            storeParam.Add(CommonStr.StoreParams.StoreDiscountPolicy, new DiscountPolicy(discountType));
            storeParam.Add(CommonStr.StoreParams.StorePuarchsePolicy, new PuarchsePolicy(puarchseType));
            Store store = new Store(storeParam);

            Tuple<bool, string> ownershipAdded = user.addStoreOwnership(store);

            if (!ownershipAdded.Item1)
            {
                nextStoreId -= 1;
                return new Tuple<int, string>(-1, ownershipAdded.Item2);
            }
            else
            {
                stores.Add(nextStoreId, store);
                return new Tuple<int, string>(nextStoreId, "");
            }

        }


        // impl on next version only!
        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            User user = userManager.GetAtiveUser(userName);
            if (user == null)
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistOrActiveUserErrMessage);

            if (!stores.ContainsKey(storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage);


            if (!isMainOwner(user, storeId))
                return new Tuple<bool, string>(false, CommonStr.StoreMangmentErrorMessage.notMainOwnerErrMessage);


            if (!user.isStoreOwner(storeId))
                return new Tuple<bool, string>(false, "user" + userName + " not store owner of " + storeId.ToString());

            Tuple<bool, string> ownerShipedRemoved = userManager.removeAllFromStore(storeId);
            if (!ownerShipedRemoved.Item1)
                return ownerShipedRemoved;

            stores.Remove(storeId);

            return new Tuple<bool, string>(true, "");
        }

        private bool isMainOwner(User user, int storeId)
        {
            return stores[storeId].isMainOwner(user);
        }




        public void cleanup()
        {
            this.stores = new Dictionary<int, Store>();
        }
    }
}
