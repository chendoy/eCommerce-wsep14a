using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    class External
    {
        // User Component
        // function that would check if this is a valid user and return the User object for further
        public static bool CheckValidUser(string user)
        {
            return true;
        }


        // All this functions can be at Store object that I will get first from CheckValidStore

        // Store Component
        public static bool CheckValidStore(string store)
        {
            return true;
        }

        // Inventory Component
        public static int GetAmountOfProduct(string store, string product)
        {
            return Int32.MaxValue;
        }

        public static bool CheckValidProduct(string store, string product)
        {
            return true;
        }

        public static Tuple<bool, string> CheckValidBasketForStore(string store, Dictionary<string, int> products)
        {
            return new Tuple<bool, string>(true, "");
        }

        public static int GetBasketPrice(string store, Dictionary<string, int> products)
        {
            return 10000;
        }
    }
}
