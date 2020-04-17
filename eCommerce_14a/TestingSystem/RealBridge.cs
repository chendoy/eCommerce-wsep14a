using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;
using eCommerce_14a.PurchaseComponent.ServiceLayer;
using eCommerce_14a.UserComponent.ServiceLayer;
using eCommerce_14a.StoreComponent.ServiceLayer;

namespace TestingSystem
{
    class RealBridge : BridgeInterface
    {
        Appoitment_Service appointService; //sundy
        UserService userService; //sundy
        System_Service sysService; //sundy
        StoreService StoreService; //liav
        PurchaseService purchService; //naor

        public RealBridge()
        {
            appointService = new Appoitment_Service();
            userService = new UserService();
            sysService = new System_Service("admin", "admin");
            StoreService = new StoreService();
            purchService = new PurchaseService();
        }

        /// ~~~~~~~~Naor~~~~~~~~:

        // TODO: resolve return type 
        //public new List<object> ViewCartDetails(string cartID)
        //{
        //    return purchService.GetCartDetails(cartID);
        //}
       
        public new Tuple<bool, string> AddProductToBasket(string userID, int storeID, int productID, int amount)
        {
            return purchService.AddProductToShoppingCart(userID, storeID, productID, amount);
        }

        public new Tuple<bool, string> PayForProduct(string userID, string paymentDetails, string address)
        {
            return purchService.PerformPurchase(userID, paymentDetails, address);//@@@@@@@@@@@@@@@@@@
        }

        public new Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)//@@@@@@@@@@@@@@@@@@@@
        {
            return purchService.PerformPurchase(user, paymentDetails, address);
        }

        // TODO: resolve return type 
        //public new Tuple<List<object>, string> ViewPurchaseUserHistory(string userName)
        //{
        //    return purchService.GetBuyerHistory(userName);//@@@@@@@@@@@@
        //}

        // TODO: resolve return type 
        //public new Tuple<List<object>, string> ViewAllStorePurchase(string userName, int storeID) //@@@@@@@
        //{
        //    return purchService.GetStoreHistory(userName, storeID);
        //}


        public new Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return purchService.RemoveProductFromShoppingCart(user, store, product);
        }


        /// ~~~~~~~~Liav~~~~~~~~:
        
        //TODO: change to increase decrase
        //public new Tuple<bool, String> ChangeProductAmount(int storeID, string username, int productID, int newAmount)
        //{
        //    return StoreService.ChangeProductAmount(storeID, username, productID, newAmount);//@@@@@@@@@@@@
        //}

        public new Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return StoreService.appendProduct(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount); ;
        }

        public new Dictionary<string, object> ViewStoreDetails(int storeID)
        {
            return StoreService.getStoreInfo(storeID);
        }


        // TODO: resolve return type 
        //public new Dictionary<int, List<object>> ViewStoreProductsByCategory(int storeID, String category)
        //{
        //    return StoreService.SearchProducts(new Dictionary<string, object> { { "searchByCategory", category } });//@@@@@@@@@@@@@@@@@
        //}

        public new Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return StoreService.removeStore(username, storeID);
        }


        // TODO: resolve return type 
        //public new Dictionary<int, List<object>> ViewProductByName(String productName)
        //{
        //    return StoreService.SearchProducts(new Dictionary<string, object> { { "SearchByProductName", productName } });//@@@@@@@@@@@@@@@@@
        //}

        public new Tuple<int, string> OpenStore(string userName)
        {
            return StoreService.createStore(userName, 0, 0); // without discount or buying policy
        }

        public new Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return StoreService.UpdateProduct(userId, storeId, productId, newDetails, price, name, category);
        }

        public new Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return StoreService.removeProduct(storeID, username, productID);
        }

        // impl on next version
        //public new Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID)
        //{
        //    return StoreService.CheckBuyingPolicy(userID, storeID);// @@@@@@@@@@@@@@ stub
        //}

        //impl on next version
        //public new Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID)
        //{
        //    return StoreService.CheckDiscountPolicy(userID, storeID); //@@@@@@@@@@@@@@@@@@@@ stub
        //}

        public new void ClearAllShops()
        {
            StoreService.cleanup();
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
            return sysService.initSystem("Admin","Admin");
        }

        // TODO: change to <bool, string>
        //public new Tuple<string, string> enterSystem()
        //{
        //    return new Tuple<string,string>(userService.LoginAsGuest(),"");
        //}

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
