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

        public bool Registration(string username,string password)
        {
            return UM.Register(username,password);
        }
        public bool Login(string username, string password)
        {
            return UM.Login(username, password);
        }
        public bool LoginAsGuest()
        {
            return UM.Login("", "",true);
        }
        public bool Logout(string user)
        {
            return UM.Logout(user);
        }

    }
}
