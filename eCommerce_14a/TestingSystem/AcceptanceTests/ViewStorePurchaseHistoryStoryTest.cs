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
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
        string paymentDetails = "33225554";
        string address = "hanesher3";

        [TestInitialize]
        public void SetUp()
        {
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
            PerformPurchase(username ,paymentDetails, address);
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
            Assert.IsNull(ViewAllStorePurchase(" ", storeID).Item1, ViewAllStorePurchase(" ", storeID).Item2);
        }
    }
}
