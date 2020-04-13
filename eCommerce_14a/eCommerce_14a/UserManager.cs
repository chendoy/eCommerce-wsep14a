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

        //Checks if user name and password are legit and not exsist
        private Tuple<bool, string> name_and_pass_check(string u, string p)
        {
            if (u == null || p == null)
                return new Tuple<bool, string>(false, "Null Arguments\n");
            if (u == "" || p == "")
                return new Tuple<bool, string>(false, "Blank Arguments\n");
            if (isUserExist(u))
                return new Tuple<bool, string>(false, "User name Exsist\n");
            return new Tuple<bool, string>(true, "");
        }
        //Check 1 or 2 arguments a=if input is valid.
        private Tuple<bool, string> check_args(string u, string p = "A")
        {
            if (u == null || p == null)
                return new Tuple<bool, string>(false, "Null Arguments\n");
            if (u == "" || p == "")
                return new Tuple<bool, string>(false, "Blank Arguments\n");
            return new Tuple<bool, string>(true, "");
        }
        //CHecks if the user is registered somewhere 
        //Means the user entered username and password
        public bool isUserExist(string username)
        {
            //if user name exist return false
            return Users_And_Hashes.ContainsKey(username);
        }
        //Register the system admin
        public Tuple<bool, string> RegisterMaster(string username, string pass)
        {
            Tuple<bool, string> ans = name_and_pass_check(username, pass);
            if (!ans.Item1)
                return ans;
            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username, sha1);
            User System_Admin = new User(0, username, false, true);
            users.Add(username, System_Admin);
            return new Tuple<bool, string>(true, "");
        }
        //Register regular user to the system 
        //User name must be unique
        public Tuple<bool, string> Register(string username, string pass)
        {
            Tuple<bool, string> ans = name_and_pass_check(username, pass);
            if (!ans.Item1)
                return ans;
            string sha1 = SB.CalcSha1(pass);
            Users_And_Hashes.Add(username, sha1);
            User nUser = new User(Available_ID, username, false);
            users.Add(username, nUser);
            Available_ID++;
            return new Tuple<bool, string>(true, "");
        }
        //Login to Unlogged Register User with valid user name and pass.
        public Tuple<bool, string> Login(string username, string pass, bool isGuest = false)
        {
            if (isGuest)
            {
                addGuest();
                return new Tuple<bool, string>(true, "");
            }
            Tuple<bool, string> ans = check_args(username, pass);
            if (!ans.Item1)
                return ans;
            string sha1 = SB.CalcSha1(pass);
            string temp_pass_hash;
            if (!Users_And_Hashes.TryGetValue(username, out temp_pass_hash))
                return new Tuple<bool, string>(false, "No such User: " + username + "\n");
            if (Users_And_Hashes.ContainsKey(username) && temp_pass_hash == sha1)
            {
                User tUser;
                if (!users.TryGetValue(username, out tUser))
                    return new Tuple<bool, string>(false, "Error occured User is not in the users_DB but is registered: " + username + " \n");
                if (tUser.LoggedStatus())
                    return new Tuple<bool, string>(false, "The user: " + username + " is already logged in\n");
                tUser.LogIn();
                Active_users.Add(tUser.getUserName(), tUser);
                return new Tuple<bool, string>(true, username + " Logged int\n");
            }
            return new Tuple<bool, string>(false, "Wrong Credentials\n");

        }
        public Tuple<bool, string> Logout(string sname, User user = null)
        {
            Tuple<bool, string> ans = check_args(sname);
            if (!ans.Item1)
                return ans;
            if (user is null)
                user = GetAtiveUser(sname);
            if (user == null)
                return new Tuple<bool, string>(false, sname + "is not Logged in\n");
            if (!user.LoggedStatus())
                return new Tuple<bool, string>(false, sname + "is not Logged in\n");
            if (user.isguest())
                return new Tuple<bool, string>(false, "Guest cannot Log out.\n");
            user.Logout();
            Active_users.Remove(user.getUserName());
            addGuest();
            return new Tuple<bool, string>(true, sname + " Logged out succesuffly\n");
        }
        //Add Guest user to the system and to the relevant lists.
        private void addGuest()
        {
            string tName = "Guest" + Available_ID;
            User nUser = new User(Available_ID, tName);
            Console.WriteLine(tName);
            nUser.LogIn();
            Active_users.Add(tName, nUser);
            Available_ID++;
        }
        //Tries to get User from users list
        public User GetUser(string username)
        {
            User tUser;
            if (users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }
        //Tries to get user from logged in users.
        public User GetAtiveUser(string username)
        {
            User tUser;
            if (Active_users.TryGetValue(username, out tUser))
                return tUser;
            return null;
        }
        //Function for Tests only add to active user (simulate login) 
        //And Remove from active users (Simulate logout).
        public void addtoactive(User u)
        {
            Active_users.Add(u.getUserName(), u);
            users.Add(u.getUserName(), u);
        }
        public void Rtoactive(User u)
        {
            Active_users.Remove(u.getUserName());
            users.Add(u.getUserName(), u);
        }

        public bool isMainOwner(User user, int storeId)
        {
            //function should check if it is the user who created this store (the main owner)
            throw new NotImplementedException();
        }

        public Tuple<bool, string> removeAllFromStore(int storeId)
        {
            //function should remove all store owners and store mangers from this store
            throw new NotImplementedException();
        }
        public Tuple<bool, string> removeOwner(Store store ,User user)
        {
            //when you delete a store owner of store you should also delete it from the store owner list and use the function below
            throw new NotImplementedException();
            // the function is here ...
            store.RemoveOwner(user);
        }



    }


}