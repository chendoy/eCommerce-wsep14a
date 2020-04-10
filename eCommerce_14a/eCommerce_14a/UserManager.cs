using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace eCommerce_14a
{

    public class UserManager
    {
        private Dictionary<string, string> Users_And_Hashes;
        private Dictionary<string, User> users;
        private Dictionary<string, User> Active_users;
        private int Available_ID;
        public UserManager()
        {
            Console.WriteLine("UserManager Created\n");
            Users_And_Hashes = new Dictionary<string, string>();
            users = new Dictionary<string, User>();
            Active_users = new Dictionary<string, User>();
            Available_ID = 1;
        }
        public UserManager getManagerInstance()
        {
            return this;
        }

        public bool isUserExist(string username)
        {
            //if user name exist return false
            return Users_And_Hashes.ContainsKey(username);
        }
        public bool RegisterMaster(string username, string sha1)
        {
            if (username == null || sha1 == null || username == "" || sha1 == "")
                return false;
            Users_And_Hashes.Add(username,sha1);
            User System_Admin = new User(0, username, false, true);
            users.Add(username,System_Admin);
            return true;
        }
        public bool Register(string username, string sha1)
        {
            if(username == null || sha1 == null || isUserExist(username) || sha1=="" || username == "")
            {
                return false;
            }
            Users_And_Hashes.Add(username, sha1);
            User nUser = new User(Available_ID, username, false);
            users.Add(username,nUser);
            Available_ID++;
            return true;
        }
        public bool Login(string username, string sha1,bool isGuest = false)
        {
            if(isGuest)
            {
                addGuest();
                return true;
            }
            string temp_pass_hash;
            if (!Users_And_Hashes.TryGetValue(username, out temp_pass_hash))
            {
                Console.WriteLine("User Do Not Exist\n");
                return false;
            }
            if(Users_And_Hashes.ContainsKey(username) && temp_pass_hash == sha1)
            {
                User tUser;
                if(!users.TryGetValue(username,out tUser))
                {
                    Console.WriteLine("Error occured User is not in the users_DB but is registered\n");
                    return false;
                }
                tUser.LogIn();
                Active_users.Add(tUser.getUserName(), tUser);
                Console.WriteLine("User Logged in Successfully.\n");
                return true;
            }
            return false;

        }
        public bool Logout(string sname,User user = null)
        {
            if (sname == "" || sname is null)
                return false;
            if (user is null)
            {
                user = GetAtiveUser(sname);
            }
            if (user.isguest())
            {
                Console.WriteLine("Guest cannot Log out.\n");
                return false;
            }
            if (!user.LoggedStatus())
            {
                Console.WriteLine("Error user is not logged in so he cannot Log out.\n");
                return false;
            }
            user.Logout();
            Active_users.Remove(user.getUserName());
            Console.WriteLine("Log out success\n");
            Console.WriteLine("Log in as Guest\n");
            addGuest();
            return true;

        }
        private void addGuest()
        {
            string tName = "Guest" + Available_ID;
            User nUser = new User(Available_ID, tName);
            Console.WriteLine(tName);
            nUser.LogIn();
            Active_users.Add(tName, nUser);
            Available_ID++;
        }
        public User GetUser(string username)
        {
            User tUser;
            if (users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }
        public User GetAtiveUser(string username)
        {
            User tUser;
            if (Active_users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }

        //Temp Function
        public bool OpenStore()
        {
            User nUser = new User(99, "test", false);
            return nUser.openStore(1);

        }
    }
}
