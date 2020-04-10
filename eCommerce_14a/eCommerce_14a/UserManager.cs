using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace eCommerce_14a
{
    //Add service calss that calls all this functions 
    // No Logic in the service class and call this function from user manager   
    public class UserManager
    {
        private Dictionary<string, string> Users_And_Hashes;
        private Dictionary<string, User> users;
        private Dictionary<string, User> Active_users;
        private int Available_ID;
        private Security SB;
        public UserManager()
        {
            Console.WriteLine("UserManager Created\n");
            Users_And_Hashes = new Dictionary<string, string>();
            users = new Dictionary<string, User>();
            Active_users = new Dictionary<string, User>();
            SB = new Security();
            Available_ID = 1;
        }
        public UserManager getManagerInstance()
        {
            return this;
        }

        private bool name_and_pass_check(string u,string p)
        {
            if (u == null || p == null || u == "" || p == "" || isUserExist(u))
                return false;
            return true;
        }
        private bool check_args(string u, string p = "A")
        {
            if (u == null || p == null || u == "" || p == "")
                return false;
            return true;
        }
        public bool isUserExist(string username)
        {
            //if user name exist return false
            return Users_And_Hashes.ContainsKey(username);
        }
        
        public bool RegisterMaster(string username, string pass)
        {
            if (!name_and_pass_check(username, pass))
                return false;
            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username,sha1);
            User System_Admin = new User(0, username, false, true);
            users.Add(username,System_Admin);
            return true;
        }
        public bool Register(string username, string pass)
        {
            if (!name_and_pass_check(username, pass))
                return false;
            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username, sha1);
            User nUser = new User(Available_ID, username, false);
            users.Add(username,nUser);
            Available_ID++;
            return true;
        }
        public bool Login(string username, string pass,bool isGuest = false)
        {
            if(isGuest)
            {
                addGuest();
                return true;
            }
            if (!check_args(username, pass))
                return false;
            string sha1 = SB.CalcSha1(pass);
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
                if (tUser.LoggedStatus())
                    return false;
                tUser.LogIn();
                Active_users.Add(tUser.getUserName(), tUser);
                Console.WriteLine("User Logged in Successfully.\n");
                return true;
            }
            return false;

        }
        public bool Logout(string sname,User user = null)
        {
            if (!check_args(sname))
                return false;
            if (user is null)
            {
                user = GetAtiveUser(sname);
            }
            if (user == null || !user.LoggedStatus())
            {
                Console.WriteLine("Error user is not logged in so he cannot Log out.\n");
                return false;
            }
            if (user.isguest())
            {
                Console.WriteLine("Guest cannot Log out.\n");
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
        public void addtoactive(User u)
        {
            Active_users.Add(u.getUserName(),u);
        }
        public void Rtoactive(User u)
        {
            Active_users.Remove(u.getUserName());
        }
    }
}
