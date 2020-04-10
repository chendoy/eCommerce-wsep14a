using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class User
    {
        private int id;
        public User(int id)
        {
            this.id = id;
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }

}
