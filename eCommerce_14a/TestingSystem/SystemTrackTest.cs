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
       public BridgeInterface sys;

        public SystemTrackTest()
        {
            sys = Driver.GetBridge();
        }

        public Boolean Login(String username, String password)
        {
            return sys.Login(username, password);
        }

        public Boolean Register(String username, String password)
        {
            return sys.Register(username, password);
        }

        public Boolean Init() 
        {
            return sys.Init(); // TODO : impl, what are the params
        }

        public void GetDetailsFromUser() { }

        public void UserLeft() { }

        public bool UserNotRegistered()
        {
            return sys.UserNotRegistered();
        }

        public bool UserNotLogin()
        {
            return sys.UserNotLogin();
        }
        public bool ViewShopDetails()
        {
            return sys.ViewShopDetails();
        }

        public bool ViewProductsByCategory(String InvalidCategory) 
        {
            return sys.ViewProductsByCategory(InvalidCategory);
        }

        public bool CloseShop()
        {
            return sys.CloseShop();
        }

        public bool ViewProductByName(String productName)
        {
            return sys.ViewProductByName(productName);
        }

        public bool ChangeProductName(String anotherValidName)
        {
            return sys.ChangeProductName(anotherValidName);
        }
        public bool ViewProductDetails()
        {
            return sys.ViewProductDetails();
        }

        public String GetProductName()
        {
            return sys.GetProductName();
        }
    }
}
