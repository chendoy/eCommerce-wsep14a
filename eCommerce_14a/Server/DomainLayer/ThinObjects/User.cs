using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainLayer.ThinObjects
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }


        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public void clear()
        {
            this.Username = "";
            this.Password = "";
        }
    }

}
