using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DomainLayer.ThinObjects
{
    public class PurchaseBasket
    {

        public Store Store { get; set; }
        public string Username { get; set; }
        public int Price { get; set; }
        public DateTime PurchaseTime { get; set; }
        public Dictionary<int, int> Product { get; set; }

        public PurchaseBasket() { }
    }
}
