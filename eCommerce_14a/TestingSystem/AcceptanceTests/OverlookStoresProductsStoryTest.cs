﻿    using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
    [TestClass]
    public class OverlookStoresProductsStoryTest : SystemTrackTest
    {
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;
        int amount = 1;
        int productID = 3;
        string productDetails = "Details";
        double productPrice = 3.02;
        string productName = "Name";
        string productCategory = "Category";

        [TestInitialize]
        public void SetUp()
        {
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
        }
        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
            ClearAllShops();
        }

        [TestMethod]
        public void ViewShopDetailsTest() 
        {
            AddProductToStore(storeID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.AreNotEqual(0, ViewStoreDetails(storeID).Count);
        }
        [TestMethod]
        public void UnExistCategoryTest()
        {
            Assert.AreEqual(0, ViewStoreDetails(storeID).Count);// suppose to return an EmptyList
        }
        [TestMethod]
        public void CloseShopWhileViewShopDetailsTest()
        {
            CloseStore(username, storeID);
            Assert.AreEqual(0, ViewStoreDetails(storeID).Count);
        }
    }
}
