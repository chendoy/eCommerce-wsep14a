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
            UM = new UserManager();
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
        public bool Registration(string username,string password)
        {
            return UM.Register(username,password);
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
        public bool Login(string username, string password)
        {
            return UM.Login(username, password);
        }
        public bool LoginAsGuest()
        {
            return UM.Login("", "",true);
        }
        /// <req>https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer-logout-31</req>
        public bool Logout(string user)
        {
            return UM.Logout(user);
        }

    }
}
