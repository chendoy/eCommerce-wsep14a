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
        UserManager u_test = UserManager.Instance;
        Security bd = new Security();
        //UserManagerTest
        [TestMethod]
        public void MasterRegistrationTest()
        {
            //Test
            //Regular master Registration
            Assert.IsTrue(u_test.RegisterMaster("test", "Test1").Item1);
            //Null Arguments master Registration
            Assert.IsFalse(u_test.RegisterMaster("test", null).Item1);
            Assert.IsFalse(u_test.RegisterMaster(null, null).Item1);
            //Same user name Registration
            Assert.IsFalse(u_test.RegisterMaster("test", "Test1").Item1);
            //Blank args 
            Assert.IsFalse(u_test.RegisterMaster("", "Test1").Item1);
            //Checks that registration was successfull in the User DB
            Assert.IsTrue(u_test.GetUser("test").isSystemAdmin());
            Assert.IsTrue(u_test.isUserExist("test"));
            
        }
        [TestMethod]
        public void RegularRegistrationTest()
        {
            //Test
            //Regular Registration
            Assert.IsTrue(u_test.Register("test4", "Test1").Item1);
            //Exsisting user name Registration
            Assert.IsFalse(u_test.Register("test", "Test1").Item1);
            Assert.IsFalse(u_test.Register("test4", "Test1").Item1);
            //Checks that registration was successfull in the User DB
            Assert.IsTrue(u_test.isUserExist("test4"));
            //Null args
            Assert.IsFalse(u_test.Register("test", null).Item1);
            Assert.IsFalse(u_test.Register(null, null).Item1);
            //Blank args
            Assert.IsFalse(u_test.Register("", "Test1").Item1);
            
        }
        [TestMethod]
        public void LoginTest()
            {
            //setup test and test4 is already registered
            //Tests
            //Regular login
            Assert.IsTrue(u_test.Login("test", "Test1").Item1);
            //Check that user is now logged in
            Assert.IsTrue(u_test.GetAtiveUser("test").LoggedStatus());
            //Wrong Credentials login
            Assert.IsFalse(u_test.Login("test4", "A").Item1);
            //Regular user login
            Assert.IsTrue(u_test.Login("test4", "Test1").Item1);
            //Login aggain
            Assert.IsFalse(u_test.Login("test", "Test1").Item1);
            //Null and blank args
            Assert.IsFalse(u_test.Login("test", null).Item1);
            Assert.IsFalse(u_test.Login("", "A").Item1);
            Assert.IsFalse(u_test.Login(null, null).Item1);
        }
        [TestMethod]
        public void LoginAsGuestTest()
        {
            //Tests
            Assert.IsTrue(u_test.Login("", "", true).Item1);
            //Guest to because test4 took id 1.
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").LoggedStatus());
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").isguest());

        }
        [TestMethod]
        public void LogoutTest()
        {
            //pre
            UserManager.Instance.cleanup();
            u_test.Register("test", "Test1");
            u_test.Login("test", "Test1");
            u_test.Login("test", "Test1", true);
            //Tests
            //No such user guest1 there is only Guest2.
            Assert.IsFalse(u_test.Logout("Guest1").Item1);
            //Check Guest2 Created and logged in
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").isguest());
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").LoggedStatus());
            //Blanck or null args
            Assert.IsFalse(u_test.Logout("").Item1);
            Assert.IsFalse(u_test.Logout(null).Item1);
            //Regular logout
            Assert.IsTrue(u_test.Logout("test").Item1);
            //CHeck that he is logged out from DB.
            Assert.IsFalse(u_test.GetUser("test").LoggedStatus());
            Assert.IsNull(u_test.GetAtiveUser("test"));
            //Try log out again
            Assert.IsFalse(u_test.Logout("test").Item1);
            //Guest cannot logout
            Assert.IsFalse(u_test.Logout("Guest2").Item1);
            Assert.IsTrue(u_test.GetAtiveUser("Guest2").LoggedStatus());
            //Guest3 created after test logout
            Assert.IsTrue(u_test.GetAtiveUser("Guest3").isguest());
        }
    }
}
