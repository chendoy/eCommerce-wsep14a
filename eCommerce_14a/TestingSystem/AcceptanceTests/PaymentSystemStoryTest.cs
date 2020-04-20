using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-external-payment-system-7 </req>
    [TestClass]
    public class PaymentSystemStoryTest : SystemTrackTest
    {
        string paymentDetails = "ValidPaymentDetails";
        string address = "hanesher38";
        string userID;

        [TestInitialize]
        public void SetUp()
        {
            userID = enterSystem().Item2;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void LegalPaymentDetailsTest()
        {
            Assert.IsTrue(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }

        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(PayForProduct(userID, "", address).Item1, PayForProduct(userID, "", address).Item2);
        }

        [TestMethod]
        //bad
        public void ConnectionLostWithPaymentSystemTest()
        {
            SetPaymentSystemConnection(false);
            Assert.IsFalse(PayForProduct(userID, paymentDetails, address).Item1, PayForProduct(userID, paymentDetails, address).Item2);
        }
    }
}
