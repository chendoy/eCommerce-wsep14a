using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    class storeService
    {
        StoreManagment storeManagment;
        public storeService(StoreManagment storeManagment)
        {
            this.storeManagment = storeManagment;

        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }

        public Tuple<bool, string> appendProduct(int storeId, string userName, int productId, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return storeManagment.appendProduct(storeId, userName, productId, productDetails, productPrice, productName, productCategory, amount);
        }

        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {
            return storeManagment.removeProduct(storeId, userName, productId);
        }
        public Tuple<bool, string> addProduct(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.addProductAmount(storeId, userName, productId, amount);
        }

        public Tuple<bool, string> decraseProduct(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.decraseProduct(storeId, userName, productId, amount);
        }

        public Tuple<bool, string> createStore(int storeId, string userName, int discountPolicyType, int puarchsePolicyType)
        {
            return storeManagment.createStore(storeId, userName, discountPolicyType, puarchsePolicyType);
        }

        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            return storeManagment.removeStore(userName, storeId);
        }

        public Tuple<bool, string> changeStoreStatus(int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(storeId, status);
        }
        //For Admin Uses
        public void cleanup()
        {
            storeManagment.cleanup();
        }


    }
}
