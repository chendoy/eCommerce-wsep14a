using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    
    public class User
    {
        private string name;
        private int id;
        private bool isGuest;
        private int[] roles;
        private bool isAdmin,isLoggedIn;
        private Dictionary<int, Store> Store_Ownership;
        private Dictionary<int, Store> Store_Managment;
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
            AppointedBy = new Dictionary<int,User>();
            //Cart = new List<PurchaseBasket>();
            //Purchases = new List<Purchase>();
        }
        public void LogIn()
        {
            this.isLoggedIn = true;
        }
        public void Logout()
        {
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
            return this.isLoggedIn;
        }
        public bool isguest()
        {
            return this.isGuest;
        }
        public void BrowseCart()
        {
            //BrowseCart.ForEach(Print)
        }
        public void SeePurchaseHistory()
        {
            //Purchases.ForEach(Print)
        }
        public bool addStoreOwnership(Store store)
        {
            if (isguest())
                return false;
            if(Store_Ownership.ContainsValue(store))
            {
                Console.WriteLine("Error occured user already ownes the store but store do not see him as owner\n");
                return false;
            }
            Store_Ownership.Add(store.getStoreId(), store);
            return true;
        }
        public bool addStoreManagment(Store store)
        {
            if (isguest())
                return false;
            if (Store_Managment.ContainsValue(store))
            {
                Console.WriteLine("Error occured user already Manages the store but store do not see him as Manager\n");
                return false;
            }
            Store_Managment.Add(store.getStoreId(), store);
            return true;
        }
        public bool isAppointedBy(User user, int store_id)
        {
            User owner;
            if (!AppointedBy.TryGetValue(store_id, out owner))
                return false;
            return owner == user;
        }
        public void addAppointment(User owner, int id)
        {
            if (!isAppointedBy(owner, id))
                AppointedBy.Add(id, owner);
        }
        public bool RemoveStoreManagment(int store_id)
        {
            return Store_Managment.Remove(store_id);
        }
        public bool RemoveAppoitment(User owner,int id)
        {
            return AppointedBy.Remove(id);
        }
        public bool isStoreOwner(int store)
        {
            return Store_Ownership.ContainsKey(store);
        }
        public bool isStorManager(int store)
        {
            return Store_Managment.ContainsKey(store);
        }
        public bool isSystemAdmin()
        {
            return this.isAdmin;
        }

        public bool openStore(int v)
        {
            Store n = new Store(v);
            return true;
        }
    }
}
