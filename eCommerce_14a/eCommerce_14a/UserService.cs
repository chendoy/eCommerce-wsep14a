using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class UserService
    {
        UserManager UM;
        public UserService()
        {
            UM = UserManager.Instance;
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
        public bool Registration(string username, string password)
        {
            Tuple<bool, string> ans = UM.Register(username, password);
            Console.WriteLine(ans.Item2);
            return ans.Item1;
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
        public bool Login(string username, string password)
        {
            Tuple<bool, string> ans = UM.Login(username, password);
            Console.WriteLine(ans.Item2);
            return ans.Item1;
        }
        public string LoginAsGuest()
        {
            Tuple<bool, string> ans = UM.Login("", "", true);
            Console.WriteLine(ans.Item2);
            return ans.Item2;
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer-logout-31</req>
        public bool Logout(string user)
        {
            Tuple<bool, string> ans = UM.Logout(user);
            Console.WriteLine(ans.Item2);
            return ans.Item1;
        }
        //For Admin Usage
        public void cleanup()
        {
            UM.cleanup();
        }
    }
}