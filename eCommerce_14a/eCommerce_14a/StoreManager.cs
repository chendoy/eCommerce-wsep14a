using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    class StoreManager
    {
        StoreManagment storeManagment;
        public StoreManager(StoreManagment storeManagment)
        {
            this.storeManagment = storeManagment;
        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }

        public Tuple<bool, String> addProduct(int storeId, int userId, Product p, int amount)
        {
            return storeManagment.addProduct(storeId, userId, p, amount);
        }

        public Tuple<bool, String> decraseProduct(int storeId, int userId, Product p, int amount)
        {
            return storeManagment.decraseProduct(storeId, userId, p, amount);
        }

        public Tuple<bool, string> createStore(int storeId, int userId)
        {
            return storeManagment.createStore(storeId, userId);
        }

        public Tuple<bool, string> removeStore(int userId, int storeId)
        {
            return storeManagment.removeStore(userId, storeId);
        }

        public Tuple<bool, string> changeStoreStatus(int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(storeId, status);
        }

        public Tuple<bool, string> updateProductDetails(int storeId, int userId, int productId, string newDetails)
        {
            return storeManagment.UpdatePrdocutDetails(storeId, userId, productId, newDetails);
        }


    }
}
