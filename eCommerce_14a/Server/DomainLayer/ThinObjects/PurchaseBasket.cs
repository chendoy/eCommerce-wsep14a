using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.ClientObjects
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
