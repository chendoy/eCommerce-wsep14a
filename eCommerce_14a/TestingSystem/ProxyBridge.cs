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

        public new bool Login(String username, String password)
        {
            return true;
        }

        public new bool Register(String username, String password)
        {
            return true;
        }

        public new bool Init()
        {
            return true;
        }

        public new void GetDetailsFromUser(){}

        public new void UserLeft(){}

        public new bool UserNotRegistered() 
        {
            return true;
        }

        public new bool UserNotLogin() 
        {
            return true;
        }

        public new bool ViewShopDetails()
        {
            return true;
        }
        public new bool ViewProductsByCategory(String InvalidCategory)
        {
            return true;
        }
        public new bool CloseShop()
        {
            return true;
        }
        public new bool ViewProductByName(String productName)
        {
            return true;
        }

        public new bool ChangeProductName(String anotherValidName)
        {
            return true;
        }
        public new bool ViewProductDetails()
        {
            return true;
        }

        public new String GetProductName()
        {
            return "bla";
        }

    }
}
