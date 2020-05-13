using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.UserComponent.Communication;

namespace eCommerce_14a.UserComponent.DomainLayer
{
    public class AppoitmentManager
    {
        //All store appointer
        StoreManagment storeManagment;
        //Publisher publisher;
        UserManager UM;
        AppoitmentManager()
        {
            UM = UserManager.Instance;
            storeManagment = StoreManagment.Instance;
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
        public void LoadAppointments()
        {
            AppointStoreManager("user4", "user1", 1);
            AppointStoreManager("user5", "user4", 2);
            AppointStoreManager("user5", "user3", 2);
            AppointStoreOwner("user4", "user2", 1);
            AppointStoreOwner("user5", "user1", 2);
            int[] perms = { 1, 1, 1, 1, 1 };
            ChangePermissions("user5", "user3", 2, perms);

        }
        //Logger.logError(CommonStr.StoreMangmentErrorMessage.nonExistingStoreErrMessage, this, System.Reflection.MethodBase.GetCurrentMethod());
        //Owner appoints addto to be Store Owner.
        public Tuple<bool, string> AppointStoreOwner(string owner, string addto, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if(store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (owner == null || addto == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (owner == "" || addto == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
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
            int[] p = { 1, 1, 1 , 1, 1};
            appointed.setPermmisions(store.getStoreId(), p);
            appointed.addAppointment(appointer, id: store.getStoreId());
            //Version 2 Addition
            Tuple<bool, string> ans = Publisher.Instance.subscribe(addto, storeId);
            if (!ans.Item1)
                return ans;
            return appointed.addStoreOwnership(store);
        }

        public void cleanup()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            storeManagment = StoreManagment.Instance;
            UM = UserManager.Instance;
            //publisher = Publisher.Instance;
        }

        //Owner appoints addto to be Store Manager.
        //Set his permissions to the store to be [1,1,0] only read and view
        public Tuple<bool, string> AppointStoreManager(string owner, string addto, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (owner == null || addto == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }
                
            if (owner == "" || addto == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            } 
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
            Tuple<bool, string> res = appointed.addStoreManagment(store);
            int[] p = {1, 1, 0, 0, 0};
            appointed.setPermmisions(store.getStoreId(), p);
            //Version 2 Addition
            Tuple<bool, string> ans = Publisher.Instance.subscribe(addto, storeId);
            if (!ans.Item1)
                return ans;
            return res;
        }
        //Remove appoitment only if owner gave the permissions to the Appointed user
        public Tuple<bool, string> RemoveAppStoreManager(string o, string m, int storeId)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (o == null || m == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (o == "" || m == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
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
            //Version 2 Addition
            Tuple<bool, string> message = Publisher.Instance.Notify(storeId, new NotifyData(m + "is not a Manager any More"));
            if (!message.Item1)
                return message;
            Tuple<bool, string> ans = Publisher.Instance.Unsubscribe(m, storeId);
            if (!ans.Item1)
                return ans;
            return new Tuple<bool, string>(manager.RemoveAppoitment(owner, store.getStoreId()), "");
        }
        public Tuple<bool, string> ChangePermissions(string ownerS, string worker, int storeId, int[] permissions)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            Store store = storeManagment.getStore(storeId);
            if (store is null)
            {
                return new Tuple<bool, string>(false, "Store Does not Exist");
            }
            if (ownerS == null || worker == null || permissions == null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Null Arguments");
            }

            if (ownerS == "" || worker == "")
            {
                Logger.logError(CommonStr.ArgsTypes.Empty, this, System.Reflection.MethodBase.GetCurrentMethod());
                return new Tuple<bool, string>(false, "Blank Arguemtns\n");
            }
            //What about chheck permission length.
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
