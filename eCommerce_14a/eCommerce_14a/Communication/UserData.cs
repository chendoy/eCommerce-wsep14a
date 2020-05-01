using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Utils
{
    class UserData
    {
        string username;
        string password;
        public UserData(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
