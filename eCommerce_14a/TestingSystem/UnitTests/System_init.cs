using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class System_init
    {
        [TestMethod]
        public void AssigneAdmin_Test()
        {
            UserManager um = new UserManager();
            Security b = new Security();
            //Regular_Succes
            Assert.IsTrue(um.RegisterMaster("AAA", b.CalcSha1("BBB")));
            //Sad
            Assert.IsFalse(um.RegisterMaster("", b.CalcSha1("BBB")));
            //Bad
            Assert.IsFalse(um.RegisterMaster(null, b.CalcSha1("BBB")));
        }
        [TestMethod]
        public void ConnectToHandlers()
        {
            DeliveryHandler dh = new DeliveryHandler();
            PaymentHandler ph = new PaymentHandler();
            Assert.IsFalse(dh.checkconnection(false));
            Assert.IsFalse(dh.checkconnection(false));
            Assert.IsTrue(dh.checkconnection());
            Assert.IsTrue(dh.checkconnection());
        }
        [TestMethod]
        public void SecurityTest()
        {
            Security b = new Security();
            Assert.IsNull(b.CalcSha1(""));
            Assert.IsNull(b.CalcSha1(null));
            Assert.IsNotNull(b.CalcSha1("A"));
        }
    }
}
