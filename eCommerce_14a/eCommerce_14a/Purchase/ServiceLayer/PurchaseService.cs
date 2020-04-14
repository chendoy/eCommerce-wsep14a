using eCommerce_14a.Purchase.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.ServiceLayer
{
    public class PurchaseService
    {
        private PurchaseManagement purchaseManagement = new PurchaseManagement();

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
        public Tuple<bool, string> AddProductToShoppingCart(string user, string store, string product, int amount)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, amount, false);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<Cart, string> GetCartDetails(string user)
        {
            return purchaseManagement.GetCartDetails(user);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> ChangeProductAmoountInShoppingCart(string user, string store, string product, int amount)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, amount, true);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-view-and-edit-shopping-cart-27 </req>
        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, string store, string product)
        {
            return purchaseManagement.AddProductToShoppingCart(user, store, product, 0, true);
        }

    }
}
