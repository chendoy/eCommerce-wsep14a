using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;

namespace TestingSystem
{
    class RealBridge : BridgeInterface
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        eSystem sysService; //sundy
        storeService storeService; //liav
        PurchaseService purchService; //naor

        public RealBridge()
        {
            appointService = new Appoitment_Service();
            userService = new UserService();
            sysService = new eSystem();
            storeService = new storeService();
            purchService = new PurchaseService();
        }

        /// ~~~~~~~~Naor~~~~~~~~:

        public new List<object> ViewCartDetails(string cartID)
        {
            return purchService.GetCartDetails(cartID);
        }
       
        public new Tuple<bool, string> AddProductToBasket(string userID, int storeID, int productID, int amount)
        {
            return purchService.AddProductToShoppingCart(userID, storeID, productID, amount);
        }

        public new Tuple<bool, string> PayForProduct(string userID, string paymentDetails)
        {
            return purchService.PayForProduct(userID, paymentDetails);//@@@@@@@@@@@@@@@@@@
        }

        public new Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)//@@@@@@@@@@@@@@@@@@@@
        {
            purchService.PerformPurchase(user, paymentDetails, address);
        }

        public new Tuple<List<object>, string> ViewPurchaseUserHistory(string userName)
        {
            return purchService.GetBuyerHistory(userName);//@@@@@@@@@@@@
        }

        public new Tuple<List<object>, string> ViewAllStorePurchase(string userName, int storeID) //@@@@@@@
        {
            return purchService.GetStoreHistory(userName, storeID);
        }

        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return purchService.RemoveProductFromShoppingCart(user, store, product);
        }


        /// ~~~~~~~~Liav~~~~~~~~:

        public new Tuple<bool, String> ChangeProductAmount(int storeID, string username, int productID, int newAmount)
        {
            return storeService.ChangeProductAmount(storeID, username, productID, newAmount);//@@@@@@@@@@@@
        }

        public new Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return storeService.appendProduct(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount); ;
        }

        public new Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return storeService.getStoreInfo(storeID);
        }

        public new Dictionary<int, List<object>> ViewStoreProductsByCategory(int storeID, String category)
        {
            return storeService.SearchProducts(new Dictionary<string, object> { { "searchByCategory", category } });//@@@@@@@@@@@@@@@@@
        }

        public new Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return storeService.removeStore(username, storeID);
        }

        public new Dictionary<int, List<object>> ViewProductByName(String productName)
        {
            return storeService.SearchProducts(new Dictionary<string, object> { { "SearchByProductName", productName } });//@@@@@@@@@@@@@@@@@
        }

        public new Tuple<int, string> OpenStore(string userName)
        {
            return storeService.createStore(userName, 0, 0); // without discount or buying policy
        }

        public new Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return storeService.UpdateProduct(userId, storeId, productId, newDetails, price, name, category);
        }

        public new Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return storeService.removeProduct(storeID, username, productID);
        }

        public new Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID)
        {
            return storeService.CheckBuyingPolicy(userID, storeID);// @@@@@@@@@@@@@@ stub
        }

        public new Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID)
        {
            return storeService.CheckDiscountPolicy(userID, storeID); //@@@@@@@@@@@@@@@@@@@@ stub
        }

        public new void ClearAllShops()
        {
            storeService.cleanup();
        }


        /// ~~~~~~~~Sundy~~~~~~~~:

        public new void SetSupplySystemConnection(bool v)
        {
            sysService.SetDeliveryConnection(v);
        }

        public new void SetPaymentSystemConnection(bool v)
        {
            sysService.SetPaymentConnection(v);
        }

        public new Tuple<bool, string> Logout(string userID)
        {
            return userService.Logout(userID);
        }

        public new Tuple<bool, string> Login(String username, String password)
        {
            return userService.Login(username, password);
        }

        public new Tuple<bool, string> Register(String username, String password)
        {
            return userService.Registration(username, password);
        }

        public new Tuple<bool, string> Init()
        {
            return sysService.system_init("Admin","Admin");
        }

        public new Tuple<string, string> enterSystem()
        {
            return new Tuple<string,string>(userService.LoginAsGuest(),"");
        }

        public new Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag) //@@@@@@@@@
        {
            return sysService.ProvideDeliveryForUser(UserID, paymentFlag);
        }

        public new Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return appointService.AppointStoreOwner(owner, appoint, store);
        }

        public new Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return appointService.AppointStoreManage(owner, appoint, store);
        }

        public new Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return appointService.ChangePermissions(owner, appoint, store, permissions);
        }

        public new Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return appointService.RemoveStoreManager(owner, appoint, store);
        }

        public new void ClearAllUsers()
        {
            userService.cleanup();
        }
    }
}
