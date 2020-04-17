using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;

namespace eCommerce_14a.UserComponent.DomainLayer
{
    public class AppoitmentManager
    {
        //All store appointer
        StoreManagment storeManagment;
        UserManager UM;
        AppoitmentManager()
        {
            UM = UserManager.Instance;
            Console.WriteLine("AppoitmentManager Created\n");
        }
        private static readonly object padlock = new object();  
        private static AppoitmentManager instance = null;  
        public static AppoitmentManager Instance  
        {  
            get  
            {  
                if (instance == null)  
                {  
                    lock (padlock)  
                    {  
                        if (instance == null)  
                        {  
                            instance = new AppoitmentManager();  
                        }  
                    }  
                }  
                return instance;  
            }  
        }
        public void SetStoreMeneger(StoreManagment s)
        {
            storeManagment = s;
        }
        //Owner appoints addto to be Store Owner.
        public Tuple<bool, string> AppointStoreOwner(string owner, string addto, int storeId)
        {
            Store store = storeManagment.getStore(storeId);
            if (owner == null || addto == null || store == null)
                return new Tuple<bool, string>(false, "Null Arguments");
            if (owner == "" || addto == "")
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetUser(addto);
            if (appointer is null || appointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (appointer.isguest() || appointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(appointed))
                return new Tuple<bool, string>(false, addto + " Is already Store Owner\n");
            if (!store.IsStoreOwner(appointer))
                return new Tuple<bool, string>(false, owner + "Is not a store Owner\n");
            store.AddStoreOwner(appointed);
            int[] p = { 1, 1, 1 };
            appointed.setPermmisions(store.getStoreId(), p);
            appointed.addAppointment(appointer, id: store.getStoreId());
            return appointed.addStoreOwnership(store);
        }

        public void cleanup()
        {
            UM = UserManager.Instance;
        }

        //Owner appoints addto to be Store Manager.
        //Set his permissions to the store to be [1,1,0] only read and view
        public Tuple<bool, string> AppointStoreManager(string owner, string addto, int storeId)
        {
            Store store = storeManagment.getStore(storeId);
            if (owner == null || addto == null || store == null)
                return new Tuple<bool, string>(false, "Null Arguments");
            if (owner == "" || addto == "")
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            User appointer = UM.GetAtiveUser(owner);
            User appointed = UM.GetUser(addto);
            if (appointer is null || appointed is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (appointer.isguest() || appointed.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(appointed) || store.IsStoreManager(appointed))
                return new Tuple<bool, string>(false, addto + " Is already Store Owner or Manager\n");
            if (!store.IsStoreOwner(appointer))
                return new Tuple<bool, string>(false, owner + "Is not a store Owner\n");
            store.AddStoreManager(appointed);
            appointed.addAppointment(appointer, store.getStoreId());
            int[] p = { 1, 1, 0 };
            appointed.setPermmisions(store.getStoreId(), p);
            return appointed.addStoreManagment(store);
        }
        //Remove appoitment only if owner gave the permissions to the Appointed user
        public Tuple<bool, string> RemoveAppStoreManager(string o, string m, int storeId)
        {
            Store store = storeManagment.getStore(storeId);
            if (o == null || m == null || store == null)
                return new Tuple<bool, string>(false, "Null Arguments");
            if (o == "" || m == "")
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            User owner = UM.GetAtiveUser(o);
            User manager = UM.GetUser(m);
            if (owner is null || manager is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (owner.isguest() || manager.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(manager))
                return new Tuple<bool, string>(false, m + " Is already Store Owner\n");
            if (!store.IsStoreOwner(owner))
                return new Tuple<bool, string>(false, o + "Is not a store Owner\n");
            if (!manager.isAppointedBy(owner, store.getStoreId()))
                return new Tuple<bool, string>(false, m + "Is not appointed by " + o + "to be store manager\n");
            store.RemoveManager(manager);
            manager.RemoveStoreManagment(store.getStoreId());
            manager.RemovePermission(store.getStoreId());
            return new Tuple<bool, string>(manager.RemoveAppoitment(owner, store.getStoreId()), "");
        }
        public Tuple<bool, string> ChangePermissions(string ownerS, string worker, int storeId, int[] permissions)
        {
            Store store = storeManagment.getStore(storeId);
            if (ownerS == null || worker == null || permissions == null || store == null)
                return new Tuple<bool, string>(false, "Null Arguments");
            if (ownerS == "" || worker == "")
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            User owner = UM.GetAtiveUser(ownerS);
            User manager = UM.GetUser(worker);
            if (owner is null || manager is null)
                return new Tuple<bool, string>(false, "One of the users is not logged Exist\n");
            if (owner.isguest() || manager.isguest())
                return new Tuple<bool, string>(false, "One of the users is a Guest\n");
            if (store.IsStoreOwner(manager))
                return new Tuple<bool, string>(false, worker + " Is already Store Owner\n");
            if (!store.IsStoreOwner(owner))
                return new Tuple<bool, string>(false, ownerS + "Is not a store Owner\n");
            if (!manager.isAppointedBy(owner, store.getStoreId()))
                return new Tuple<bool, string>(false, worker + "Is not appointed by " + ownerS + "to be store manager\n");
            return manager.setPermmisions(store.getStoreId(), permissions);
        }
        //Temp function for tests
        //Add user to logged in list and Remove user from logged in lists.
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
