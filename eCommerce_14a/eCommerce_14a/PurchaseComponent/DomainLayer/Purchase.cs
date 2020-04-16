using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class Purchase
    {
        public string User { get; private set; }
        public DateTime PurchaseTime { get; private set; }
        public Cart UserCart { get; private set; }

        public Purchase(string user, DateTime purchaseTime, Cart userCart)
        {
            User = user;
            PurchaseTime = purchaseTime;
            UserCart = userCart;
        }
    }
}
