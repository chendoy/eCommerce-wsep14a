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
        int productID = 3;
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        string userManager = UserGenerator.GetValidUsernames()[1];
        string passManager = UserGenerator.GetPasswords()[1];
        int storeID;
        int amount = 1;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
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
            Assert.IsTrue(AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item1,
                AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount).Item2);
        }

        [TestMethod]
        //sad
        public void AddTwiceProductToStoreTest()
        {
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
