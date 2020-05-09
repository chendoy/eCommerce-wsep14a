using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainLayer.ThinObjects
{
    public class UserData
    {
        public string Username { get; set; }

        public UserData() { }


        public UserData(string Username)
        {
            this.Username = Username;
        }

        public void clear()
        {
            this.Username = "";
        }
    }

}
