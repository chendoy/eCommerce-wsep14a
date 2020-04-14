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

        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
        {
            return storeManagment.UpdateProduct(userName, storeId, productId, pDetails, pPrice, pName, pCategory);
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

        public Tuple<int, string> createStore(string userName, int discountPolicyType, int puarchsePolicyType)
        {
            return storeManagment.createStore(userName, discountPolicyType, puarchsePolicyType);
        }

        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            return storeManagment.removeStore(userName, storeId);
        }

        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(userName, storeId, status);
        }

        public int getProductAmount(int storeId, int productId)
        {
            return storeManagment.getProductAmount(storeId: storeId, productId: productId); 
        }




    }
}
