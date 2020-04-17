using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.PurchaseComponent.DomainLayer
{
    public class External
    {
        // User Component
        // function that would check if this is a valid user and return the User object for further
        public static bool CheckValidUser(string user)
        {
            return true;
        }

        public static bool CheckOwnership(string manager, int storeId)
        {
            throw new NotImplementedException();
        }

        public static bool CheckIsAdmin(string admin)
        {
            throw new NotImplementedException();
        }

        static StoreExternal store = new StoreExternal();
        // All this functions can be at Store object that I will get first from CheckValidStore
        // Store Component
        // public Store getStore(int storeId)
        // active store property - if not close
        public static StoreExternal CheckValidStore(int store)
        {
            return External.store;
        }


    }


    public class StoreExternal
    {
        public int Id { get; } // storeId
        // Inventory Component
        // public Tuple<Product, int> getProductDetails(int productId) . item2
        public int GetAmountOfProduct(int product)
        {
            return Int32.MaxValue;
        }

        //public bool productExist(int productId)
        public bool CheckValidProduct(int product)
        {
            return true;
        }

        // Gets Dictionary of Product ID and Amount
        // public Tuple<bool, string> checkIsValidBasket(Dictionary<int, int> products)
        // check if products are valid and amounts
        public Tuple<bool, string> CheckValidBasket(Dictionary<int, int> products)
        {
            return new Tuple<bool, string>(true, "");
        }

        // Gets Dictionary of Product ID and Amount
        // public double getBucketPrice(Dictionary<int, int> products)
        public int GetBasketPrice(Dictionary<int, int> products)
        {
            return 10000;
        }
    }
}
