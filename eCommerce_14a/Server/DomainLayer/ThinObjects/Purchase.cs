using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.ClientObjects
{
    public class Purchase
    {
        public string Username { get; set; }
        public Cart Cart { get; set; }

        public Purchase() { }
    }
}
