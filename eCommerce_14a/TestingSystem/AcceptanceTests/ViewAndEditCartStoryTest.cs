using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class ViewAndEditCartStoryTest : SystemTrackTest
    {
        Cart notEmptyCart;
        Cart emptyCart;
        [TestInitialize]
        public void SetUp()
        {
            notEmptyCart = CartGenerator.GetNotEmptyCart();
            emptyCart = CartGenerator.GetEmptyCart();
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }
        [TestMethod]
        public void ViewShoppingCartTest() 
        {
            //ViewShoppingCart();
            Assert.AreNotEqual(0, notEmptyCart.ViewDetails().Count);
        }



    }
}
