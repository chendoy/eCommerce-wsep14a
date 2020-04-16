using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.Purchase.DomainLayer
{
    public class External
    {
        // User Component
        // function that would check if this is a valid user and return the User object for further
        public static bool CheckValidUser(string user)
        {
            return true;
        }

        static Store store = new Store();
        // All this functions can be at Store object that I will get first from CheckValidStore
        // Store Component
        public static Store CheckValidStore(int store)
        {
            return External.store;
        }
    }


    public class Store
    {
        public int Id { get; }
        // Inventory Component
        public int GetAmountOfProduct(int product)
        {
            return Int32.MaxValue;
        }

        public bool CheckValidProduct(int product)
        {
            return true;
        }

        // Gets Dictionary of Product ID and Amount
        public Tuple<bool, string> CheckValidBasket(Dictionary<int, int> products)
        {
            return new Tuple<bool, string>(true, "");
        }

        // Gets Dictionary of Product ID and Amount
        public int GetBasketPrice(Dictionary<int, int> products)
        {
            return 10000;
        }
    }
}
