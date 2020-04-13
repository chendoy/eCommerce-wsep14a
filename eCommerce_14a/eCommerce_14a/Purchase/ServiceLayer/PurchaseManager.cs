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
        public Tuple<bool, string> AddProductToShoppingCart(string user, string store, string product, int amount)
        {
            return purchaseFacade.AddProductToShoppingCart(user, store, product, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Dictionary<string, PurchaseBasket>, string> GetCartDetails(string user)
        {
            return purchaseFacade.GetCartDetails(user);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> ChangeProductAmoountInShoppingCart(string user, string store, string product, int amount)
        {
            return purchaseFacade.AddProductToShoppingCart(user, store, product, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, string store, string product)
        {
            return purchaseFacade.AddProductToShoppingCart(user, store, product, 0);
        }

        public Tuple<bool, string> ClearUserCart(string user)
        {
            return purchaseFacade.ClearUserCart(user);
        }
    }
}
