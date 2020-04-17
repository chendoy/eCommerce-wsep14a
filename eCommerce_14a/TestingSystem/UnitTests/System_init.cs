using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a;
using eCommerce_14a.UserComponent.DomainLayer;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class System_init
    {
        [TestMethod]
        public void AssigneAdmin_Test()
        {
            UserManager um = UserManager.Instance;
            um.cleanup();
            //Regular_Succes
            Tuple<bool, string> ans;
            ans = um.RegisterMaster("AAA", "BBB");
            Assert.IsTrue(ans.Item1);
            //Null or blank args
            Assert.IsFalse(um.RegisterMaster("", null).Item1);
            //Bad
            Assert.IsFalse(um.RegisterMaster(null,"").Item1);
        }
        [TestMethod]
        public void ConnectToHandlers()
        {
            DeliveryHandler dh = new DeliveryHandler();
            PaymentHandler ph = new PaymentHandler();
            dh.setConnection(false);
            ph.setConnections(false);
            Assert.IsFalse(dh.checkconnection());
            Assert.IsFalse(dh.checkconnection());
            dh.setConnection(true);
            ph.setConnections(true);
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
