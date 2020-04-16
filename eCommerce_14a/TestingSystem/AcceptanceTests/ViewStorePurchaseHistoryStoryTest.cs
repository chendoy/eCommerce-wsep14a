using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-purchases-history-view-410 </req>
    [TestClass]
    public class ViewStorePurchaseHistoryStoryTest : SystemTrackTest
    {
        string username;
        int storeID;
        string password;

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ViewAllPurchaseTest() 
        {
            // TODO: add purchase before
            Assert.AreNotEqual(0, ViewAllStorePurchase(username, storeID).Item1.Count);
        }

        [TestMethod]
        //sad
        public void EmptyPurchaseHisstoryTest() 
        {
            Assert.AreEqual(0, ViewAllStorePurchase(username, storeID).Item1.Count);
        }

        [TestMethod]
        //bad
        public void BlankUsernamePurchaseHisstoryTest()
        {
            Assert.AreEqual(0, ViewAllStorePurchase(" ", storeID).Item1.Count);
        }
    }
}
