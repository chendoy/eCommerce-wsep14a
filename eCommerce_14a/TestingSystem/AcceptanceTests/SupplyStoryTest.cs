using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-supply-system-send-products-284 </req>
    [TestClass]
    public class SupplyStoryTest : SystemTrackTest
    {
        string userID;

        [TestInitialize]
        public void SetUp()
        {
            Init();
            userID = enterSystem().Item2;
        }

        [TestCleanup]
        public void TearDown()
        {
            SetPaymentSystemConnection(true);
            SetSupplySystemConnection(true);
            Logout(userID);
            ClearAllUsers();
        }

        //[TestMethod]
        ////happy
        //public void LegalSupplyDetailsTest()
        //{
        //    Assert.IsTrue(ProvideDeliveryForUser(userID, true).Item1, ProvideDeliveryForUser(userID, true).Item2);
            
        //}

        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(ProvideDeliveryForUser(userID, false).Item1, ProvideDeliveryForUser(userID, false).Item2);
        }

        [TestMethod]
        //bad
        public void ConnectionLostWithSupplySystemTest()
        {
            SetSupplySystemConnection(false);
        Assert.IsFalse(ProvideDeliveryForUser(userID, true).Item1, ProvideDeliveryForUser(userID, true).Item2);
        }
    }
}
