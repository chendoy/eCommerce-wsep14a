using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---remove-product-514- </req>
    [TestClass]
    class ManagerRemoveProductStoryTest : SystemTrackTest
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
        int anotherStoreID;

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
            anotherStoreID = OpenStore(username).Item1;
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
        public void RemoveValidProductFromStoreTest()
        {
            amount = 1;
            AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(RemoveProductFromStore(userManager, storeID, productID).Item1, RemoveProductFromStore(userManager, storeID, productID).Item2);
        }

        [TestMethod]
        //sad
        public void RemoveUnExistingProductFromStoreTest()
        {
            Assert.IsFalse(RemoveProductFromStore(userManager, storeID, productID).Item1, RemoveProductFromStore(userManager, storeID, productID).Item2);
        }

        [TestMethod]
        //bad
        public void RemoveProductFromAnotherStoreTest()
        {
            amount = 1;
            AddProductToStore(anotherStoreID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(RemoveProductFromStore(userManager, storeID, productID).Item1, RemoveProductFromStore(userManager, storeID, productID).Item2);
        }
    }
}
