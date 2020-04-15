using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class Purchase
    {
        private Cart userCart;

        public Purchase(Cart userCart)
        {
            this.userCart = userCart;
        }
    }
}
