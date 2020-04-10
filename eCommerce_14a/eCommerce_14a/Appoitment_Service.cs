using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Appoitment_Service
    {
        AppoitmentManager AM;
        public Appoitment_Service()
        {
            AM = new AppoitmentManager();
        }
        //Need to be store number but i need Liav
        public bool AppointStoreOwner(string owner,string appoint,Store store)
        {
            return AM.AppointStoreOwner(owner,appoint,store);
        }
        public bool AppointStoreManage(string owner, string appoint, Store store)
        {
            return AM.AppointStoreManager(owner, appoint, store);
        }
        public bool RemoveStoreManager(string owner, string appoint, Store store)
        {
            return AM.RemoveAppStoreManager(owner, appoint, store);
        }
        public bool ChangePermissions(string owner, string appoint, Store store,int[] permissions)
        {
            return AM.ChangePermissions(owner, appoint, store,permissions);
        }
    }
}
