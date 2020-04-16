using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-pay-for-products-283 </req>
    [TestClass]
    public class PaymentStoryTest :SystemTrackTest
    {
        //@@@@@@@@@@@@@@@@@@@@@@NAOR@@@@@@@@@@@@@@@@@@@@@@@@@@@
        string paymentDetails;
        string userID;

        [TestInitialize]
        public void SetUp()
        {
            paymentDetails = "ValidPaymentDetails";
            userID = enterSystem().Item1;
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }
        [TestMethod]
        //happy
        public void LegalPaymentDetailsTest() 
        {
            Assert.IsTrue(PayForProduct(userID, paymentDetails).Item1, PayForProduct(userID, paymentDetails).Item2);
        }
        [TestMethod]
        //sad
        public void IllegalPaymentDetailsTest()
        {
            Assert.IsFalse(PayForProduct(userID, "").Item1, PayForProduct(userID, "").Item2);
        }
        [TestMethod]
        //bad
        public void ConnectionLostWithPaymentSystemTest()
        {
            setPaymentSystemConnection(false);
            Assert.IsFalse(PayForProduct(userID, paymentDetails).Item1, PayForProduct(userID, paymentDetails).Item2);
        }
    }
}
