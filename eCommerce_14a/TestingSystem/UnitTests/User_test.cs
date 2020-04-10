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
        [TestMethod]
        public void UserManagerTest()
        {
            //setup
            UserManager u_test = new UserManager();
            Security bd = new Security();
            string t_hash = bd.CalcSha1("Test1");

            

            //RegisterMaster test
            Assert.IsTrue(u_test.RegisterMaster("test", t_hash));
            Assert.IsFalse(u_test.RegisterMaster("test", null));
            Assert.IsTrue(u_test.GetUser("test").isSystemAdmin());
            Assert.IsFalse(u_test.RegisterMaster("", t_hash));
            Assert.IsFalse(u_test.RegisterMaster(null, null));
            //Register test
            Assert.IsTrue(u_test.RegisterMaster("test4", t_hash));
            Assert.IsFalse(u_test.Register("test", t_hash));
            Assert.IsFalse(u_test.Register("test", null));
            Assert.IsFalse(u_test.Register("", t_hash));
            Assert.IsFalse(u_test.Register(null, null));
            //isUserExist test
            Assert.IsTrue(u_test.isUserExist("test"));
            Assert.IsFalse(u_test.isUserExist("tes"));
            //Login test
            Assert.IsTrue(u_test.Login("test", t_hash));
            Assert.IsTrue(u_test.GetUser("test").LoggedStatus());
            Assert.IsTrue(u_test.Login("test4", t_hash));
            Assert.IsTrue(u_test.GetUser("test4").LoggedStatus());
            Assert.IsFalse(u_test.Login("tes", "A"));
            Assert.IsFalse(u_test.Login("test", "A"));
            Assert.IsTrue(u_test.Login("", "", true));
            Assert.IsTrue(u_test.GetAtiveUser("Guest1").isguest());
            //Logout test
            Assert.IsFalse(u_test.Logout("Guest1"));
            Assert.IsFalse(u_test.Logout(""));
            Assert.IsFalse(u_test.Logout(null));
            Assert.IsTrue(u_test.Logout("test4"));
            Assert.IsFalse(u_test.GetUser("test4").LoggedStatus());
            Assert.IsFalse(u_test.Logout("test4",u_test.GetUser("test4")));

            //OpenStore test
            //good
            //bad
        }
        [TestMethod]
        public void User_Test()
        {
            //Pre
            Store tmpS = new Store(5);
            User tmpU = new User(8, "owner", false);
            User GuestU = new User(8, "ownGer", true);
            //guest tests
            Assert.IsTrue(GuestU.isguest());
            Assert.IsFalse(tmpU.isguest());
            //Store Ownership
            Assert.IsTrue(tmpU.addStoreOwnership(tmpS));
            Assert.IsFalse(tmpU.addStoreOwnership(tmpS));
            //Sys Adm
            Assert.IsFalse(tmpU.isSystemAdmin());
            //Store Managment
            Assert.IsTrue(tmpU.addStoreManagment(tmpS));
            Assert.IsFalse(tmpU.addStoreManagment(tmpS));
            //Guest store Managment
            Assert.IsFalse(GuestU.addStoreOwnership(tmpS));
            Assert.IsFalse(GuestU.addStoreManagment(tmpS));
            //Remove Manager
            Assert.IsTrue(tmpU.RemoveStoreManagment(tmpS.getStoreId()));
            Assert.IsTrue(tmpU.isStoreOwner(tmpS.getStoreId()));
            tmpU.addAppointment(tmpU,tmpS.getStoreId());
            Assert.IsTrue(tmpU.isStoreOwner(tmpS.getStoreId()));
            //Remove appoitment
            Assert.IsTrue(tmpU.RemoveAppoitment(tmpU, tmpS.getStoreId()));
            //Check that stats removed
            Assert.IsFalse(tmpU.isStorManager(tmpS.getStoreId()));
            Assert.IsFalse(tmpU.isSystemAdmin());
            Assert.IsFalse(tmpU.RemoveAppoitment(tmpU, tmpS.getStoreId()));
        }
        [TestMethod]

        public void Store_Test()
        {
            //Pre
            Store tmpS = new Store(5);
            User tmpU = new User(8, "owner4", false);
            User GuestU = new User(8, "ownGer4", true);
            Assert.IsTrue(tmpS.AddStoreManager(tmpU));
            //Fuck this up
            Assert.IsFalse(tmpS.AddStoreManager(tmpU));
            Assert.IsFalse(tmpS.AddStoreManager(GuestU));
            Assert.IsTrue(tmpS.AddStoreOwner(tmpU));
            Assert.IsFalse(tmpS.AddStoreOwner(tmpU));
            Assert.IsFalse(tmpS.AddStoreOwner(GuestU));
            Assert.IsTrue(tmpS.IsStoreManager(tmpU));
            Assert.IsTrue(tmpS.IsStoreOwner(tmpU));
            Assert.IsFalse(tmpS.IsStoreOwner(GuestU));
            Assert.IsFalse(tmpS.IsStoreManager(GuestU));
            Assert.IsTrue(tmpS.RemoveManager(tmpU));
            //Remove dont work need to check this
            Assert.IsFalse(tmpS.IsStoreManager(tmpU));
            Assert.IsFalse(tmpS.RemoveManager(GuestU));
            Assert.IsFalse(tmpS.RemoveManager(tmpU));

        }

    }
}
