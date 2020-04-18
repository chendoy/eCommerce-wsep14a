using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    class ProxyBridge : BridgeInterface
    {
        public ProxyBridge() { }
        public new Tuple<bool, string> Login(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> Register(String username, String password)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> Init()
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return new Dictionary<string, object>();
        }

        public new List<object> ViewStoreProductsByCategory(int storeID, String InvalidCategory)
        {
            return new List<object>();
        }

        public new Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new List<object> ViewProductByName(String productName)
        {
            return new List<object>();
        }

        public new List<object> ViewProductDetails(int productID)
        {
            return new List<object>();
        }

        public new List<object> ViewCartDetails(string id)
        {
            return new List<object>();
        }

        public new Tuple<string, string> enterSystem()
        {
            return new Tuple<string, string>("", "");
        }

        public new Tuple<bool, string> PayForProduct(string userID, string paymentDetails)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> CheckValidInventory(string userID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new void SetPaymentSystemConnection(bool v)
        {
            return;
        }

        public new Tuple<bool, string> Logout(string userID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new void SetSupplySystemConnection(bool v)
        {
            return;
        }

        public new void ClearAllUsers()
        {
            return;
        }

        public new Tuple<int, string> OpenStore(string userName)
        {
            return new Tuple<int, String>(1, "");
        }

        public new void ClearAllShops()
        {
            return;
        }

        public new void PerformPurchase(string userName)
        {
            return;
        }

        public new List<object> ViewPurchaseUserHistory(string userName)
        {
            return new List<object>();
        }

        public new Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, String> ChangeProductAmount(int storeID, string username, int productID, int newAmount)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return new Tuple<bool, String>(true, "");
        }

        public new List<object> ViewAllStorePurchase(string userName, int storeID)
        {
            return new List<object>();
        }

        public new Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return new Tuple<bool, String>(true, "");
        }

    }
}
