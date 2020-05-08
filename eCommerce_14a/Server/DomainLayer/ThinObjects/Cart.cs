using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainLayer.ThinObjects
{
    public class Cart
    {
        public string Username { get; set; }
        public List<PurchaseBasket> baskets { get; set; }

        public Cart() { }
    }
}
