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
    public class StoreProductsInBasketStoryTest : SystemTrackTest
    {
        Product validProduct;
        Product invalidProduct;
        Basket basket;
        [TestInitialize]
        public void SetUp()
        {
            basket = BasketGenerator.GetValidBasket();
            validProduct = ProductGenerator.GetValidProduct();
            invalidProduct = ProductGenerator.GetInvalidProduct();
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }
        [TestMethod]
        //happy
        public void StoreAvailableProductTest() 
        {
            basket.AddProduct(validProduct);
            Assert.IsTrue(basket.Contains(validProduct));
        }
        [TestMethod]
        //bad
        public void StoreUnAvailableProductTest()
        {
            basket.AddProduct(invalidProduct);
            Assert.IsFalse(basket.Contains(validProduct));
        }

        [TestMethod]
        //sad
        public void StoreProductWhileShopCloseTest()
        {
            basket.AddProduct(validProduct);
            CloseShop();
            Assert.IsFalse(basket.Contains(validProduct));
        }


    }
}
