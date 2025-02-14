﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---remove-product-514- </req>
    [TestClass]
    public class ManagerRemoveProductStoryTest : SystemTrackTest
    {
        int productID = 1;
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
        int anotherStoreID;

        [TestInitialize]
        public void SetUp()
        {
            Register(userManager, passManager);
            Login(userManager, passManager);
            Register(username, password);
            Login(username, password);
            storeID = OpenStore(username).Item1;
            anotherStoreID = OpenStore(username).Item1;
            AppointStoreManage(username, userManager, storeID);
            ChangePermissions(username, userManager, storeID, new int[] { 1, 1, 1 , 0, 0});
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
            AddProductToStore(anotherStoreID, username, productID, productDetails, productPrice, productName, productCategory, amount);
            Assert.IsFalse(RemoveProductFromStore(userManager, storeID, productID).Item1, RemoveProductFromStore(userManager, storeID, productID).Item2);
        }
    }
}
