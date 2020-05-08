using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Client.Data
{
    public class User
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        // Roles[0] is the user's current role to present in client UI (e.g in NavBar)
        public string[] Roles { get; set; }

        public User() { }


        public User(string Username, string Password, string[] Roles)
        {
            this.Username = Username;
            this.Password = Password;
            this.Roles = Roles;
        }

        public void clear()
        {
            this.Username = "";
            this.Password = "";
        }

        public static implicit operator string(User v)
        {
            throw new NotImplementedException();
        }
    }

}
