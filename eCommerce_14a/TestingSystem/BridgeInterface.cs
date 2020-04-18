using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    //this is the empty bridge for abstraction
    public class BridgeInterface
    {
        public BridgeInterface() { }

        public Tuple<bool, string> Login(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> Register(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> Init()
        {
            return new Tuple<bool, String>(true, "");
        }

        public Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return new Dictionary<string, object>();
        }

        public List<object> ViewStoreProductsByCategory(int storeID, String InvalidCategory)
        {
            return new List<object>();
        }

        public Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Dictionary<int, List<object>> ViewProductByName(String productName)
        {
            return new Dictionary<int, List<object>>();
        }

        public List<object> ViewCartDetails(string id) 
        {
            return new List<object>();
        }

        public Tuple<string, string> enterSystem() 
        {
            return new Tuple<string, string>("", "");
        }

        public Tuple<bool, string> PayForProduct(string userID, string paymentDetails)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public void SetPaymentSystemConnection(bool v)
        {
            return;
        }

        public Tuple<bool, string> Logout(string userID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag) 
        {
            return new Tuple<bool, String>(true, "");
        }

        public void SetSupplySystemConnection(bool v) 
        {
            return;
        }

        public void ClearAllUsers() 
        {
            return;
        }

        public Tuple<int, string> OpenStore(string userName)
        {
            return new Tuple<int, String>(1, "");
        }

        public void ClearAllShops()
        {
            return;
        }

        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<List<object>, string> ViewPurchaseUserHistory(string userName)
        {
            return new Tuple<List<object>, string>(new List<object>(), "");
        }

        public Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, String> ChangeProductAmount(int storeID, string username, int productID, int newAmount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public Tuple<List<object>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return new Tuple<List<object>, string>(new List<object>(), "");
        }

        public Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }
        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return new Tuple<bool, String>(true, "");
        }
    }
}
