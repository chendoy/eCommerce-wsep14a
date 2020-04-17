    using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--history-37 </req>
    [TestClass]
    public class ViewUserPurchaseHistoryStoryTest : SystemTrackTest
    {
        string username;
        string password;
        string paymentDetails = "311546777";
        string address = "han";
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AddProductToStore(storeID, username, 3, "lego", 3.0, "lego", "building", 2);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ViewValidHistoryTest() 
        {
            AddProductToBasket(username, storeID, 3, 1);
            PerformPurchase(username, paymentDetails, address);
            Assert.AreNotEqual(0, ViewPurchaseUserHistory(username).Item1.Count);
        }

        [TestMethod]
        //sad
        public void ViewNoHistoryTest()
        {
            Assert.AreEqual(0, ViewPurchaseUserHistory(username).Item1.Count);
        }

        [TestMethod]
        //bad
        public void ViewInvalidDetailsHistoryTest()
        {
            Assert.AreEqual(0, ViewPurchaseUserHistory("   ").Item1.Count);
        }
    }
}
