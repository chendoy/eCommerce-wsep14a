using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager--add-product-512- </req>
    [TestClass]
    class ManagerAddProductStoryTest : SystemTrackTest
    {
        int productID;
        string username;
        string password;
        string userManager;
        string passManager;
        int storeID;
        int amount;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            productID = 3;
            userManager = UserGenerator.RandomString(5);
            passManager = UserGenerator.RandomString(5);
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(userManager, passManager);
            Login(userManager, passManager);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            AppointStoreManage(username, userManager, storeID);
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ValidProductAddToStoreTest()
        {
            amount = 1;
            Assert.IsTrue(AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
        }

        [TestMethod]
        //sad
        public void AddTwiceProductToStoreTest()
        {
            amount = 1;
            Assert.IsTrue(AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
            Assert.IsFalse(AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
        }

        [TestMethod]
        //bad
        public void AddProductWithNegAmountToStoreTest()
        {
            amount = -1;
            Assert.IsFalse(AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
        }
    }
}
