using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Utils
{
    class UserData
    {
        public int Opcode { get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserData(string username, string password)
        {
            Opcode = 1;
            Username = username;
            Password = password;
        }

    }
}
