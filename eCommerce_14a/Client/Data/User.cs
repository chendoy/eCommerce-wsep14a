using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Client.Data
{
    public class User
    {
        //[Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public User()
        {
        }

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
