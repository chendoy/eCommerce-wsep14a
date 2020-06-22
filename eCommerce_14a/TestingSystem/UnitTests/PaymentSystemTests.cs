using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class PaymentSystemTests
    {

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.IsAlive()"
        [TestMethod]
        public void PaymentSystemHandshaketest()
        {
            bool res = PaymentSystem.IsAlive();
            Assert.IsTrue(res);
        }

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.Pay(string, int, int, string, string, string)"
        [TestMethod]
        public void SuccessfulPaymentTest()
        {
            int res = PaymentSystem.Pay("5440988565421665", 11, 2019, "Israel Israeli", "400", "56080138");
            Assert.IsTrue (res >= 10000 && res <= 100000);
        }

        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.CancelPayment(int)"
        [TestMethod]
        public void SuccessfulCancelPaymentTest()
        {
            int res = PaymentSystem.CancelPayment(90914);
            Assert.AreEqual(res, 1);
        }
    }
}
