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

        public Boolean Login(String username, String password)
        {
            return true;
        }

        public Boolean Register(String username, String password)
        {
            return true;
        }

        public Boolean Init()
        {
            return true;
        }

        public void GetDetailsFromUser() { }

        public void UserLeft() { }

        public bool UserNotRegistered()
        {
            return true;
        }

        public bool UserNotLogin()
        {
            return true;
        }
        public bool ViewShopDetails()
        {
            return true;
        }

        public bool ViewProductsByCategory(String InvalidCategory)
        {
            return true;
        }

        public bool CloseShop()
        {
            return true;
        }

        public bool ViewProductByName(String productName)
        {
            return true;
        }

        public bool ChangeProductName(String anotherValidName)
        {
            return true;
        }

        public bool ViewProductDetails() 
        {
            return true;
        }

        public String GetProductName() 
        {
            return "bla";
        }
    }
}
