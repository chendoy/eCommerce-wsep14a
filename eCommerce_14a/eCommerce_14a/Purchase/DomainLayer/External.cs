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
        public static bool CheckValidUser(string user)
        {
            return true;
        }

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

        public static bool CheckValidProduct(string user, string product)
        {
            return true;
        }
    }
}
