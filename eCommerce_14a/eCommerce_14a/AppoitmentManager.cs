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
        UserManager UM;
        public AppoitmentManager()
        {
            UM = new UserManager();
            Console.WriteLine("AppoitmentManager Created\n");
        }
        public AppoitmentManager getInstance()
        {
            return this;
        }
        public bool AppointStoreOwner(string owner, string addto, Store store)
        {

            if (owner == null || addto == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetAtiveUser(addto);
            if (appointer is null || appointed is null)
            {
                return false;
            }
            if (appointer.isguest() || appointed.isguest())
            {
                Console.WriteLine("Error Guest users cannot appoint or be appointed\n");
                return false;
            }
            if (store.IsStoreOwner(appointed))
            {
                Console.WriteLine(appointed.getUserName() + " Is already store Owner\n");
                return false;
            }
            if (!store.IsStoreOwner(appointer))
            {
                Console.WriteLine(appointer.getUserName() + " Is Not A store Owner\n");
                return false;
            }
            store.AddStoreOwner(appointed);
            appointed.addAppointment(appointer, store.getStoreId());
            return appointed.addStoreOwnership(store);
        }
        public bool AppointStoreManager(string owner, string addto, Store store)
        {

            if (owner == null || addto == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetAtiveUser(addto);
            if (appointer is null || appointed is null)
            {
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
            int[] p = { 1, 1, 0 };
            appointed.setPermmisions(store.getStoreId(), p);
            return appointed.addStoreManagment(store);
        }
        public bool RemoveAppStoreManager(string o, string m, Store store)
        {
            if (o == null || m == null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            User owner = UM.GetAtiveUser(o);
            User manager = UM.GetAtiveUser(m);
            if (owner is null || manager is null)
            {
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
            if (!manager.isAppointedBy(owner, store.getStoreId()))
            {
                Console.WriteLine(owner.getUserName() + " Do not Appointed:" + manager.getUserName() + "\n");
                return false;
            }
            store.RemoveManager(manager);
            manager.RemoveStoreManagment(store.getStoreId());
            int[] p = { 0, 0, 0 };
            manager.setPermmisions(store.getStoreId(), p);
            return manager.RemoveAppoitment(owner, store.getStoreId());
        }
        public bool ChangePermissions(string owner, string worker, Store store,int[] permissions)
        {
            if (owner == null || worker == null || permissions is null || store == null)
            {
                Console.WriteLine("Error Null Arguments\n");
                return false;
            }
            User Boss = UM.GetAtiveUser(owner);
            User manager = UM.GetAtiveUser(worker);
            if (owner is null || manager is null)
            {
                return false;
            }
            if (Boss.isguest() || manager.isguest())
            {
                Console.WriteLine("Error Guest users cannot chnage permissions\n");
                return false;
            }
            if (store.IsStoreOwner(manager))
            {
                Console.WriteLine(manager.getUserName() + " Is a store Owner and cannot be Removed\n");
                return false;
            }
            if (!store.IsStoreOwner(Boss))
            {
                Console.WriteLine(Boss.getUserName() + " Is Not A store Owner\n");
                return false;
            }
            if (!manager.isAppointedBy(Boss, store.getStoreId()))
            {
                Console.WriteLine(Boss.getUserName() + " Do not Appointed:" + manager.getUserName() + "\n");
                return false;
            }
            return manager.setPermmisions(store.getStoreId(), permissions);
        }
        //Temp function for tests
        public void addactive(User a)
        {
            UM.addtoactive(a);
        }
        public void Rtoactive(User u)
        {
            UM.Rtoactive(u);
        }
    }
}
