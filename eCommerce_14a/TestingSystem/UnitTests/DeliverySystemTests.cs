using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using eCommerce_14a.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class DeliverySystemTests
    {
        /// <tests cref ="eCommerce_14a.Utils.PaymentSystem.IsAlive()"
        [TestMethod]
        public void DeliverySystemHandshaketest()
        {
            bool res = DeliverySystem.IsAlive();
            Assert.IsTrue(res);
        }

        /// <tests cref ="eCommerce_14a.Utils.DeliverySystem.Supply(string, string, string, string, string)"
        [TestMethod]
        public void SuccessfulSupplyTest()
        {
            int res = DeliverySystem.Supply("Israel Israeli", "Mivtza Nahshon 31", "Beersheva", "Israel", "7171660");
            Assert.IsTrue(res >= 10000 && res <= 100000);
        }

        /// <tests cref ="eCommerce_14a.Utils.DeliverySystem.CancelSupply(int)"
        [TestMethod]
        public void SuccessfulCancelDeliveryTest()
        {
            int res = DeliverySystem.CancelSupply(90914);
            Assert.AreEqual(res, 1);
        }
    }
}
