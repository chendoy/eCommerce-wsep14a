using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
    class ChangeProductAmountStoryTest : SystemTrackTest
    {
        int productID;
        int storeID;
        string username;
        string password;
        int amount;
        int newAmount;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            productID = 3;
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            amount = 1;
            newAmount = 2;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void ChangeValidProductAmountTest()
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(ChangeProductAmount(storeID, username, productID, newAmount).Item1, ChangeProductAmount(storeID, username, productID, newAmount).Item2);
        }

        [TestMethod]
        //sad
        public void ChangeAmountToUnExistingProductTest()
        {
            Assert.IsFalse(ChangeProductAmount(storeID, username, productID, newAmount).Item1, ChangeProductAmount(storeID, username, productID, newAmount).Item2);
        }

        [TestMethod]
        //bad
        public void ChangeAmountToNegTest()
        {
            newAmount = -1;
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(ChangeProductAmount(storeID, username, productID, newAmount).Item1, ChangeProductAmount(storeID, username, productID, newAmount).Item2);
        }
    }
}
