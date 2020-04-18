using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.UnitTests.Stubs;

namespace TestingSystem.UnitTests
{
    [TestClass]
    public class PurchaseManagementTest
    {
        private PurchaseManagement purchaseManagement;

        [TestInitialize]
        public void TestInitialize()
        {
            purchaseManagement = PurchaseManagement.Instance;
            purchaseManagement.SetupDependencies(
                new StoreManagementStub(123),
                PaymentHandler.Instance,
                DeliveryHandler.Instance,
                new UserManagerStub());
        }

        [TestCleanup]
        public void Cleanup()
        {
            purchaseManagement.ClearAll();
        }
    }
}
