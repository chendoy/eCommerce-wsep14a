using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---edit-product-412- </req>
    [TestClass]
    public class OwnerEditProductStoryTest : SystemTrackTest
    {
        int productID;
        string username;
        string password;
        int storeID;
        int amount;
        string newName;
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
            newName = "Name";
            amount = 1;
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        //happy
        public void EditValidProductTest()
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(UpdateProductDetails(storeID, username, productID, productDetails, productPrice, newName, productCategory).Item1, UpdateProductDetails(storeID, username, productID, productDetails, productPrice, newName, productCategory).Item2);
        }

        [TestMethod]
        //sad
        public void EditUnExistingProductTest()
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(UpdateProductDetails(storeID, username, 0, productDetails, productPrice, newName, productCategory).Item1, UpdateProductDetails(storeID, username, 0, productDetails, productPrice, newName, productCategory).Item2);
        }

        [TestMethod]
        //bad
        public void EditBlankDetailProductTest()
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(UpdateProductDetails(storeID, username, 0, productDetails, productPrice, "  ", productCategory).Item1, UpdateProductDetails(storeID, username, 0, productDetails, productPrice, "  ", productCategory).Item2);
        }
    }
}
