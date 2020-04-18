﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-change-store-managers-permissions-46- </req>
    [TestClass]
    public class ChangeManagerPermissionsStoryTest : SystemTrackTest
    {
        string username1;
        string password1;
        string username2;
        string password2;
        string username3;
        string password3;
        int storeID;
        int[] permissions;

        [TestInitialize]
        public void SetUp()
        {
            //user1 is the owner of the store with the storeID appoints user2
            username1 = UserGenerator.RandomString(5);
            password1 = UserGenerator.RandomString(5);
            username2 = UserGenerator.RandomString(5);
            password2 = UserGenerator.RandomString(5);
            username3 = UserGenerator.RandomString(5);
            password3 = UserGenerator.RandomString(5);
            Register(username1, password1);
            Login(username1, password1);
            Register(username2, password2);
            Login(username2, password2);
            Register(username3, password3);
            storeID = OpenStore(username1).Item1;
            permissions = new int[] { 0, 1, 0, 1 };
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllShops();
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void ChangeValidManagerPermissionsTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsTrue(ChangePermissions(username1, username2, storeID, permissions).Item1, ChangePermissions(username1, username2, storeID, permissions).Item2);
        }

        [TestMethod]
        //sad
        public void NotAnAppointerChangePermissionsTest()
        {
            AppointStoreManage(username1, username2, storeID);
            AppointStoreManage(username2, username3, storeID);
            Assert.IsFalse(ChangePermissions(username1, username3, storeID, permissions).Item1, ChangePermissions(username1, username3, storeID, permissions).Item2);
        }

        [TestMethod]
        //bad
        public void EmptyPermissionsArrayTest()
        {
            AppointStoreManage(username1, username2, storeID);
            Assert.IsFalse(ChangePermissions(username1, username2, storeID, new int[] { }).Item1, ChangePermissions(username1, username2, storeID, new int[] { }).Item2);
        }
    }
}
