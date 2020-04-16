using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-buying-policy-282 </req>
    [TestClass]
    public class BuyingPolicyStoryTest : SystemTrackTest
    {
        string guestID;
        string username;
        string password;
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            guestID = enterSystem().Item1;
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
        }

        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }

        [TestMethod]
        //happy
        public void ValidBuyingPolicyTest() 
        {
            Assert.IsTrue(CheckBuyingPolicy(guestID, storeID).Item1, CheckBuyingPolicy(guestID, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(CheckBuyingPolicy("  ", storeID).Item1, CheckBuyingPolicy("  ", storeID).Item2);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPurchaseTest()
        {
            Assert.IsFalse(CheckBuyingPolicy("  ", 10^6).Item1, CheckBuyingPolicy("  ", 10 ^ 6).Item2);
        }

    }
}
