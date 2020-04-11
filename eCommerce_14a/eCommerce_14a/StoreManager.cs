using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    class StoreManager
    {
        StoresManagment storeManagment;
        public StoreManager(StoresManagment storeManagment)
        {
            this.storeManagment = storeManagment;
        }

        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }


    }
}
