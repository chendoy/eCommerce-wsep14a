using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-appointing-store-owner-43 </req>
    [TestClass]
    public class AppointingOwnerStoryTest : SystemTrackTest
    {
        string username1;
        string password1;
        string username2;
        string password2;
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            //user1 is the owner of the store with the storeID appoints user2
            username1 = UserGenerator.RandomString(5);
            password1 = UserGenerator.RandomString(5);
            username2 = UserGenerator.RandomString(5);
            password2 = UserGenerator.RandomString(5);
            Register(username1, password1);
            Login(username1, password1);
            Register(username2, password2);
            Login(username2, password2);
            storeID = OpenStore(username1).Item1;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void AppointValidOwnerTest() 
        {
            Assert.IsTrue(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void AppointAlreadyOwnerTest()
        {
            AppointStoreOwner(username1, username2, storeID);
            Assert.IsFalse(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }

        [TestMethod]
        //bad
        public void AppointAlreadyManagerTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsFalse(AppointStoreOwner(username1, username2, storeID).Item1, AppointStoreOwner(username1, username2, storeID).Item2);
        }
    }
}
