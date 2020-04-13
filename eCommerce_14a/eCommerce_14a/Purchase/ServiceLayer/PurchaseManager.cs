using eCommerce_14a.Purchase.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.ServiceLayer
{
    public class PurchaseManager
    {
        private PurchaseFacade purchaseFacade = new PurchaseFacade();

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public bool AddProductToShoppingCart(string user, string store, string product, int amount)
        {
            Tuple<bool, string> res = purchaseFacade.AddProductToShoppingCart(user, store, product, amount);
            return res.Item1;
        }
    }
}
