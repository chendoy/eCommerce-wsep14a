using eCommerce_14a;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class CartGenerator
    {
        public static Cart GetNotEmptyCart() 
        {
            Cart unemptyCart = new Cart();
            unemptyCart.AddBasket(new Basket());
            return unemptyCart;
        }

        public static Cart GetEmptyCart() 
        {
            return new Cart();
        }
    }
}
