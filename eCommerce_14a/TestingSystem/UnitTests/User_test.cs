using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class User_test
    {
        //setup
        UserManager u_test = new UserManager();
        Security bd = new Security();
        //UserManagerTest
        [TestMethod]
        public void MasterRegistrationTest()
        {
            //Test
            Assert.IsTrue(u_test.RegisterMaster("test", "Test1"));
            Assert.IsFalse(u_test.RegisterMaster("test", null));
            Assert.IsFalse(u_test.RegisterMaster("test", "Test1"));
            Assert.IsTrue(u_test.GetUser("test").isSystemAdmin());
            Assert.IsFalse(u_test.RegisterMaster("", "Test1"));
            Assert.IsTrue(u_test.isUserExist("test"));
            Assert.IsFalse(u_test.RegisterMaster(null, null));
        }
        [TestMethod]
        public void RegularRegistrationTest()
        {
            //Test
            Assert.IsTrue(u_test.Register("test4", "Test1"));
            Assert.IsTrue(u_test.isUserExist("test4"));
            Assert.IsFalse(u_test.Register("test4", "Test1"));
            Assert.IsFalse(u_test.Register("test", null));
            Assert.IsFalse(u_test.Register("", "Test1"));
            Assert.IsFalse(u_test.Register(null, null));
        }
        [TestMethod]
        public void LoginTest()
        {
            //setup
            u_test.Register("test", "Test1");
            //Tests
            Assert.IsTrue(u_test.Login("test", "Test1"));
            Assert.IsTrue(u_test.GetAtiveUser("test").LoggedStatus());
            Assert.IsFalse(u_test.Login("test4", "A"));
            Assert.IsFalse(u_test.Login("test4", "Test1"));
            Assert.IsFalse(u_test.Login("test", "Test1"));
            Assert.IsFalse(u_test.Login("test", null));
            Assert.IsFalse(u_test.Login("", "A"));
            Assert.IsFalse(u_test.Login(null, null));
        }
        [TestMethod]
        public void LoginAsGuestTest()
        {
            //Tests
            Assert.IsTrue(u_test.Login("test", "Test1",true));
            Assert.IsTrue(u_test.GetAtiveUser("Guest1").LoggedStatus());
            Assert.IsTrue(u_test.GetAtiveUser("Guest1").isguest());

        }
        [TestMethod]
        public void LogoutTest()
        {
            //pre
            u_test.Register("test", "Test1");
            u_test.Login("test", "Test1");
            u_test.Login("test", "Test1", true);
            //Tests
            Assert.IsFalse(u_test.Logout("Guest1"));
            Assert.IsFalse(u_test.Logout(""));
            Assert.IsFalse(u_test.Logout(null));
            Assert.IsTrue(u_test.Logout("test"));
            Assert.IsFalse(u_test.GetUser("test").LoggedStatus());
            Assert.IsNull(u_test.GetAtiveUser("test"));
            Assert.IsFalse(u_test.Logout("test"));
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").isguest());
            Assert.IsTrue(u_test.GetAtiveUser("Guest3").isguest());
        }
    }
}
