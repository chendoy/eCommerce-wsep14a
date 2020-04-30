using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using eCommerce_14a.UserComponent;
using Server.UserComponent.Communication;

namespace eCommerce_14a.UserComponent.DomainLayer

{

    public class User
    {
        private string name;
        private int id;
        private bool isGuest;
        private bool isAdmin, isLoggedIn;
        private Dictionary<int, Store> Store_Ownership;
        private LinkedList<Message> unreadMessages;
        private Dictionary<int, Store> Store_Managment;
        private Dictionary<int, int[]> Store_options;
        //Contains the list of who appointed you to which store! not who you appointed to which store!
        private Dictionary<int, User> AppointedBy;
        //private List<PurchaseBasket> Cart;
        //private List<Purchase> Purchases;


        public User(int id, string name, bool isGuest = true, bool isAdmin = false)
        {
            this.id = id;
            this.name = name;
            this.isGuest = isGuest;
            this.isAdmin = isAdmin;
            this.isLoggedIn = false;
            Store_Ownership = new Dictionary<int, Store>();
            Store_Managment = new Dictionary<int, Store>();
            AppointedBy = new Dictionary<int, User>();
            Store_options = new Dictionary<int, int[]>();
            unreadMessages = new LinkedList<Message>();
            //Cart = new List<PurchaseBasket>();
            //Purchases = new List<Purchase>();
        }
        public void LogIn()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.isLoggedIn = true;
        }
        public void Logout()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.isLoggedIn = false;
        }
        public string getUserName()
        {
            return this.name;
        }
        public int getUserID()
        {
            return this.id;
        }
        public bool LoggedStatus()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.isLoggedIn;
        }
        public bool isguest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.isGuest;
        }

        public bool getUserPermission(int storeid,string permission)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if(permission is null)
            {
                Logger.logError(CommonStr.ArgsTypes.None, this, System.Reflection.MethodBase.GetCurrentMethod());
                return false;
            }
            int[] perms;
            if (!Store_options.TryGetValue(storeid, out perms))
                return false;
            if(permission.Equals(CommonStr.MangerPermission.Comments))
            {
                return perms[0] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.Puarchse))
            {
                return perms[1] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.Product))
            {
                return perms[2] == 1;
            }
            return false;
        }

        //This user will be store Owner 
        public Tuple<bool, string> addStoreOwnership(Store store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Owner\n");
            if (Store_Ownership.ContainsValue(store))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Ownership.Add(store.getStoreId(), store);
            return new Tuple<bool, string>(true, "");
        }
        //Version 2 changes
        public void AddMessage(Message notification)
        {
            this.unreadMessages.AddLast(notification);
        }

        //Add a user to be store Manager
        public Tuple<bool, string> addStoreManagment(Store store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Manager\n");
            if (Store_Managment.ContainsValue(store))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Managment.Add(store.getStoreId(), store);
            return new Tuple<bool, string>(true, "");
        }
        //Checks if User user appointed this current user to be owner or manager to this store
        public bool isAppointedBy(User user, int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            User owner;
            if (!AppointedBy.TryGetValue(store_id, out owner))
                return false;
            return owner == user;
        }
        //Add appointment from user to store
        //owner appointed current to be store manager\owner
        //If this apppointment exist do not add.
        public void addAppointment(User owner, int id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (!isAppointedBy(owner, id))
                AppointedBy.Add(id, owner);
        }
        //Remove Manager
        public bool RemoveStoreManagment(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Managment.Remove(store_id);
        }
        //Remove Appoitment
        public bool RemoveAppoitment(User owner, int id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return AppointedBy.Remove(id);
        }
        //Check if the user is Currently Store Owner
        public bool isStoreOwner(int store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Ownership.ContainsKey(store);
        }
        //Check if the user is Currently Store Manager
        public bool isStorManager(int store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_Managment.ContainsKey(store);
        }
        public bool isSystemAdmin()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.isAdmin;
        }
        //Set User permission over spesific store
        public Tuple<bool, string> setPermmisions(int store_id, int[] permission_set)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store_id < 1)
                return new Tuple<bool, string>(false, "No such Store id\n");
            if (permission_set == null)
                return new Tuple<bool, string>(false, "Null Argument\n");
            if (!isStorManager(store_id))
                return new Tuple<bool, string>(false, "The user is not Store Manager\n");
            if (Store_options.ContainsKey(store_id))
                Store_options.Remove(store_id);
            Store_options.Add(store_id, permission_set);
            return new Tuple<bool, string>(true, "");
        }
        public bool RemovePermission(int store_id)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return Store_options.Remove(store_id);
        }

  
    }
}