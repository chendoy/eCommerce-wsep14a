using eCommerce_14a;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-products-in-the-shopping-basket-26 </req>
    [TestClass]
    public class StoreProductsInBasketStoryTest : SystemTrackTest
    {
        int productID;
        int storeID;
        string userID;
        string username;
        string password;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";
        int amount = 3;

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            userID = enterSystem().Item1;
            productID = 3;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void StoreAvailableProductTest() 
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(AddProductToBasket(userID, storeID, productID, 1).Item1, AddProductToBasket(userID, storeID, productID, 1).Item2);
        }

        [TestMethod]
        //sad
        public void StoreUnExistingProductTest()
        {
            Assert.IsFalse(AddProductToBasket(userID, storeID, productID, 1).Item1, AddProductToBasket(userID, storeID, productID, 1).Item2);
        }

        [TestMethod]
        //bad
        public void StoreProductWithNegIDTest()
        {
            Assert.IsFalse(AddProductToBasket(userID, storeID, -2, 1).Item1, AddProductToBasket(userID, storeID, productID, 1).Item2);
        }
    }
}
