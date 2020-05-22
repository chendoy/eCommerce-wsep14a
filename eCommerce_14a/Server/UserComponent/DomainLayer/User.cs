using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using eCommerce_14a.UserComponent;
using Server.UserComponent.Communication;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_14a.UserComponent.DomainLayer

{

    public class User
    {   
        [Key]
        public string Name { set; get; }

        public int Id { set; get; }

        public bool IsGuest { set; get; }

        public bool IsAdmin { set; get; }

        public bool IsLoggedIn { set; get; }

        public Dictionary<int, Store> Store_Ownership { set; get; }

        public LinkedList<NotifyData> UnreadMessages { set; get; }

        public Dictionary<int, Store> Store_Managment { set; get; }
        public Dictionary<int, int[]> Store_options { set; get; }
        //Contains the list of who appointed you to which store! not who you appointed to which store!
        public Dictionary<int, User> AppointedBy { set; get; }
        //private List<PurchaseBasket> Cart;
        //private List<Purchase> Purchases;


        public User(int id, string name, bool isGuest = true, bool isAdmin = false)
        {
            Id = id;
            Name = name;
            IsGuest = isGuest;
            IsAdmin = isAdmin;
            IsLoggedIn = false;
            Store_Ownership = new Dictionary<int, Store>();
            Store_Managment = new Dictionary<int, Store>();
            AppointedBy = new Dictionary<int, User>();
            Store_options = new Dictionary<int, int[]>();
            UnreadMessages = new LinkedList<NotifyData>();
            //Cart = new List<PurchaseBasket>();
            //Purchases = new List<Purchase>();
        }

        public Dictionary<int, int[]> GetUserPermissions() 
        {
            return Store_options;
        }
        public void LogIn()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            IsLoggedIn = true;
        }

        public LinkedList<NotifyData> GetPendingMessages() 
        {
            return this.UnreadMessages;
        }

        public bool HasPendingMessages() 
        {
            return this.UnreadMessages.Count != 0;
        }

        public void RemovePendingMessage(NotifyData msg) 
        {
            this.UnreadMessages.Remove(msg);
        }
        public void RemoveAllPendingMessages()
        {
            this.UnreadMessages = new LinkedList<NotifyData>();
        }

        public void Logout()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            this.IsLoggedIn = false;
        }
        public string getUserName()
        {
            return this.Name;
        }
        public int getUserID()
        {
            return this.Id;
        }
        public bool LoggedStatus()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.IsLoggedIn;
        }
        public bool isguest()
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            return this.IsGuest;
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
            if (permission.Equals(CommonStr.MangerPermission.DiscountPolicy))
            {
                return perms[3] == 1;
            }
            if (permission.Equals(CommonStr.MangerPermission.PurachsePolicy))
            {
                return perms[4] == 1;
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
            Store_Ownership.Add(store.GetStoreId(), store);
            return setPermmisions(store.GetStoreId(), CommonStr.StorePermissions.FullPermissions);
        }
        //Version 2 changes
        public void AddMessage(NotifyData notification)
        {
            this.UnreadMessages.AddLast(notification);
        }

        //Add a user to be store Manager
        public Tuple<bool, string> addStoreManagment(Store store)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (isguest())
                return new Tuple<bool, string>(false, "Guest user cannot be store Manager\n");
            if (Store_Managment.ContainsValue(store))
                return new Tuple<bool, string>(false, getUserName() + " is already store Owner\n");
            Store_Managment.Add(store.GetStoreId(), store);
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
            return this.IsAdmin;
        }
        //Set User permission over spesific store
        public Tuple<bool, string> setPermmisions(int store_id, int[] permission_set)
        {
            Logger.logEvent(this, System.Reflection.MethodBase.GetCurrentMethod());
            if (store_id < 1)
                return new Tuple<bool, string>(false, "No such Store id\n");
            if (permission_set == null)
                return new Tuple<bool, string>(false, "Null Argument\n");
            if (!isStorManager(store_id) && !isStoreOwner(store_id))
                return new Tuple<bool, string>(false, "The user is not Store Manager or owner\n");
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