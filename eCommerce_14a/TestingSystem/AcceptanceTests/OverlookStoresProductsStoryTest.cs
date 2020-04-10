using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class OverlookStoresProductsStoryTest : SystemTrackTest
    {
        String InvalidCategory;
        [TestInitialize]
        public void SetUp()
        {
            InvalidCategory = "NoSuchCategory";
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }

        [TestMethod]
        public void ViewShopDetailsTest() 
        {
            Assert.IsTrue(ViewShopDetails());
        }
        [TestMethod]
        public void UnExistCategoryTest()
        {
            ViewShopDetails();
            Assert.Equals(new List(),ViewProductsByCategory(InvalidCategory));// suppose to return an EmptyList
        }
        [TestMethod]
        public void CloseShopWhileViewShopDetailsTest()
        {
            ViewShopDetails();
            CloseShop();
            Assert.IsFalse(ViewShopDetails());
        }



    }
}
