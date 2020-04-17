﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---edit-product-513- </req>
    [TestClass]
    class ManagerEditProductStoryTest : SystemTrackTest
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
        string newName = "newName";

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
        public void EditValidProductTest()
        {
            AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsTrue(UpdateProductDetails(storeID, userManager, productID, newName, productPrice, productName,productCategory).Item1, UpdateProductDetails(storeID, userManager, productID, newName, productPrice, productName, productCategory).Item2);
        }

        [TestMethod]
        //sad
        public void EditUnExistingProductTest()
        {
            AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(UpdateProductDetails(storeID, userManager, 0, newName, productPrice, productName, productCategory).Item1, UpdateProductDetails(storeID, userManager, 0, newName, productPrice, productName, productCategory).Item2);
        }

        [TestMethod]
        //bad
        public void EditBlankDetailProductTest()
        {
            AddProductToStore(storeID, userManager, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(UpdateProductDetails(storeID, userManager, 0, "  ", productPrice, productName, productCategory).Item1, UpdateProductDetails(storeID, userManager, 0, "  ", productPrice, productName, productCategory).Item2);
        }
    }
}
