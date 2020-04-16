using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    //from here comes all the functionality of the system
    public class SystemTrackTest
    {
        public BridgeInterface sys = Driver.GetBridge();

        public SystemTrackTest() { }

        /// ~~~~~~~~Naor~~~~~~~~:
        
        public List<object> ViewCartDetails(string cartID)
        {
            return sys.ViewCartDetails(cartID);
        }
        
        public Tuple<bool, string> AddProductToBasket(string UserID, int storeID, int productID, int amount)
        {
            return sys.AddProductToBasket(UserID, storeID, productID, amount);
        }
        
        public Tuple<bool, string> PayForProduct(string userID, string paymentDetails)
        {
            return sys.PayForProduct(userID, paymentDetails);
        }
        
        public Tuple<bool, string> PerformPurchase(string user, string paymentDetails, string address)
        {
            sys.PerformPurchase(user, paymentDetails, address);
        }


        public Tuple<List<object>, string> ViewPurchaseUserHistory(string userName)
        {
            return sys.ViewPurchaseUserHistory(userName);
        }

        public Tuple<List<object>, string> ViewAllStorePurchase(string userName, int storeID)
        {
            return sys.ViewAllStorePurchase(userName, storeID);
        }
        public Tuple<bool, string> RemoveProductFromShoppingCart(string user, int store, int product)
        {
            return sys.RemoveProductFromShoppingCart(user, store, product);
        }





        /// ~~~~~~~~Liav~~~~~~~~:

        public Tuple<bool, String> ChangeProductAmount(int storeID, string username, int productID, int newAmount)
        {
            return sys.ChangeProductAmount(storeID, username, productID, newAmount);
        }

        public Tuple<bool, string> AddProductToStore(int storeID, string username, int productID, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return sys.AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
        }
        
        public Dictionary<string,object> ViewStoreDetails(int storeID)
        {
            return sys.ViewStoreDetails(storeID);
        }
        
        public List<object> ViewStoreProductsByCategory(int storeID, String category) 
        {
            return sys.ViewStoreProductsByCategory(storeID, category);
        }
        
        public Tuple<bool, string> CloseStore(string username, int storeID)
        {
            return sys.CloseStore(username, storeID);
        }
        
        public Dictionary<int, List<object>> ViewProductByName(String productName)
        {
            return sys.ViewProductByName(productName);
        }
        
        public Tuple<int, string> OpenStore(string userName)
        {
            return sys.OpenStore(userName);
        }

        public Tuple<bool, string> UpdateProductDetails(int storeId, string userId, int productId, string newDetails, double price, string name, string category)
        {
            return sys.UpdateProductDetails(storeId, userId, productId, newDetails, price, name, category);
        }

        public Tuple<bool, string> RemoveProductFromStore(string username, int storeID, int productID)
        {
            return sys.RemoveProductFromStore(username, storeID, productID);
        }
        public Tuple<bool, string> CheckBuyingPolicy(string userID, int storeID)
        {
            return sys.CheckBuyingPolicy(userID, storeID);
        }

        public Tuple<bool, string> CheckDiscountPolicy(string userID, int storeID)
        {
            return sys.CheckDiscountPolicy(userID, storeID);
        }

        public void ClearAllShops()
        {
            sys.ClearAllShops();
        }



        /// ~~~~~~~~Sundy~~~~~~~~:

        public void SetSupplySystemConnection(bool v)
        {
            sys.SetSupplySystemConnection(v);
        }
        
        public void SetPaymentSystemConnection(bool v)
        {
            sys.SetPaymentSystemConnection(v);
        }
        
        public Tuple<bool, string> Logout(string userID)
        {
            return sys.Logout(userID);
        }
        
        public Tuple<bool, string> Login(String username, String password)
        {
            return sys.Login(username, password);
        }
        
        public Tuple<bool, string> Register(String username, String password)
        {
            return sys.Register(username, password);
        }
     
        public Tuple<bool, string> Init()
        {
            return sys.Init();
        }

        public Tuple<string, string> enterSystem()
        {
            return sys.enterSystem();
        }

        public Tuple<bool, string> ProvideDeliveryForUser(string UserID, bool paymentFlag)
        {
            return sys.ProvideDeliveryForUser(UserID, paymentFlag);
        }

        public Tuple<bool, string> AppointStoreOwner(string owner, string appoint, int store)
        {
            return sys.AppointStoreOwner(owner, appoint, store);
        }

        public Tuple<bool, string> AppointStoreManage(string owner, string appoint, int store)
        {
            return sys.AppointStoreManage(owner, appoint, store);
        }

        public Tuple<bool, string> ChangePermissions(string owner, string appoint, int store, int[] permissions)
        {
            return sys.ChangePermissions(owner, appoint, store, permissions);
        }

        public Tuple<bool, string> RemoveStoreManager(string owner, string appoint, int store)
        {
            return sys.RemoveStoreManager(owner, appoint, store);
        }
        public void ClearAllUsers()
        {
            sys.ClearAllUsers();
        }
    }
}

