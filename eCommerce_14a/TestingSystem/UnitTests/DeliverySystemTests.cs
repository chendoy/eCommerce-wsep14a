using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using eCommerce_14a.UserComponent.DomainLayer;
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
        [TestMethod]
        public void UnSuccesfullDeliveryBlankArgs()
        {
            string DeliveryDetails = "dani&&Wollurberg&&12345678";
            Tuple<bool,string> res = DeliveryHandler.Instance.ProvideDeliveryForUser(DeliveryDetails);
            Assert.IsFalse(res.Item1);
        }
        [TestMethod]
        public void UnSuccesfullDeliveryNullArgs()
        {
            Tuple<bool, string> res = DeliveryHandler.Instance.ProvideDeliveryForUser(null);
            Assert.IsFalse(res.Item1);
        }
        [TestMethod]
        public void UnSuccesfullDeliveryNotEnoughArgs()
        {
            string paymentDetails = "3333444455556666&333&222222222";
            Tuple<bool, string> res = DeliveryHandler.Instance.ProvideDeliveryForUser(paymentDetails);
            Assert.IsFalse(res.Item1);
        }
        [TestMethod]
        public void SuccesfullDelivery()
        {
            string paymentDetails = "3333444455556666&11&333&222222222&4575";
            Tuple<bool, string> res = DeliveryHandler.Instance.ProvideDeliveryForUser(paymentDetails);
            Assert.IsTrue(res.Item1);
        }
        [TestMethod]
        public void UnsuccesfullDeliveryAndRefund()
        {
            string paymentDetails = "3333444455556666&4&11&333&222222222&4568";
            int res = PaymentHandler.Instance.pay(paymentDetails);
            Assert.IsTrue(res != -1);
            string tDetails = "3333444455556666&333&222222222&4575";
            Tuple<bool, string> res2 = DeliveryHandler.Instance.ProvideDeliveryForUser(tDetails);
            Assert.IsFalse(res2.Item1);
            Tuple<bool, string> res3 = PaymentHandler.Instance.refund(res);
            Assert.IsTrue(res3.Item1);
        }
        [TestMethod]
        public void SystemIsNouTp()
        {
            string Delivery = "3333444455556666&333&222222222&4568&5";
            Tuple<bool,string> res = DeliveryHandler.Instance.ProvideDeliveryForUser(Delivery);
            Assert.IsTrue(res.Item1);
            string Delivery2 = "3333444455556666&333&222222222&4568&5";
            Tuple<bool, string> res2 = DeliveryHandler.Instance.ProvideDeliveryForUser(Delivery2,true);
            Assert.IsFalse(res2.Item1);
        }
    }
}
