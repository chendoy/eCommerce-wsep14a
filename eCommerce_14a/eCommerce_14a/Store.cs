using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Store
    {
        private int id;
        private List<User> Owners;
        private List<User> Managers;
        public Store(int id)
        {
            Owners = new List<User>();
            Managers = new List<User>();
            this.id = id;
        }
        public int getStoreId()
        {
            return this.id;
        }
        public bool IsStoreOwner(User user)
        {
            return Owners.Contains(user);
        }
        public bool AddStoreOwner(User user)
        {
            if (user.isguest() || Owners.Contains(user))
                return false;
            Owners.Add(user);
            return true;
        }
        public bool IsStoreManager(User user)
        {
            return Managers.Contains(user);
        }
        public bool AddStoreManager(User user)
        {
            if (user.isguest() || Managers.Contains(user))
                return false;
            Managers.Add(user);
            return true;
        }
        public bool RemoveManager(User user)
        {
            return Managers.Remove(user);
        }
    }
}
