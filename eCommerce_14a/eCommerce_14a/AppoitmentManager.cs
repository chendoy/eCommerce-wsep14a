using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class AppoitmentManager
    {
        //All store appointer
        public AppoitmentManager()
        {
            Console.WriteLine("AppoitmentManager Created\n");
        }
        public AppoitmentManager getInstance()
        {
            return this;
        }
        public bool AppointStoreOwner(User appointer,User appointed,Store store)
        {
            if (appointer == null || appointed == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            if (appointer.isguest() || appointed.isguest())
            {
                Console.WriteLine("Error Guest users cannot appoint or be appointed\n");
                return false;
            }
            if(store.IsStoreOwner(appointed))
            {
                Console.WriteLine(appointed.getUserName() +" Is already store Owner\n");
                return false;
            }
            if (!store.IsStoreOwner(appointer))
            {
                Console.WriteLine(appointer.getUserName() + " Is Not A store Owner\n");
                return false;
            }
            store.AddStoreOwner(appointed);
            return appointed.addStoreOwnership(store);
        }
        public bool AppointStoreManager(User appointer, User appointed, Store store)
        {
            if (appointer == null || appointed == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            if (appointer.isguest() || appointed.isguest())
            {
                Console.WriteLine("Error Guest users cannot appoint or be appointed\n");
                return false;
            }
            if (store.IsStoreOwner(appointed) || store.IsStoreManager(appointed))
            {
                Console.WriteLine(appointed.getUserName() + " Is already store Owner or Manager\n");
                return false;
            }
            if (!store.IsStoreOwner(appointer))
            {
                Console.WriteLine(appointer.getUserName() + " Is Not A store Owner\n");
                return false;
            }
            store.AddStoreManager(appointed);
            appointed.addAppointment(appointer, store.getStoreId());
            return appointed.addStoreManagment(store);
        }
        public bool RemoveAppStoreManager(User owner,User manager, Store store)
        {
            if (owner == null || manager == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            if (owner.isguest() || manager.isguest())
            {
                Console.WriteLine("Error Guest users cannot appoint or be appointed\n");
                return false;
            }
            if (store.IsStoreOwner(manager))
            {
                Console.WriteLine(manager.getUserName() + " Is a store Owner and cannot be Removed\n");
                return false;
            }
            if (!store.IsStoreOwner(owner))
            {
                Console.WriteLine(owner.getUserName() + " Is Not A store Owner\n");
                return false;
            }
            if (!manager.isAppointedBy(owner,store.getStoreId()))
            {
                Console.WriteLine(owner.getUserName() + " Do not Appointed:"+manager.getUserName()+"\n");
                return false;
            }
            store.RemoveManager(manager);
            manager.RemoveStoreManagment(store.getStoreId());
            return manager.RemoveAppoitment(owner, store.getStoreId());
        }
    }
}
