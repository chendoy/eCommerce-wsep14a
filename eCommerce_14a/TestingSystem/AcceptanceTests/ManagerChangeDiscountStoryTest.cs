using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---change-discount-511 </req>
    class ManagerChangeDiscountStoryTest : SystemTrackTest
    {
        string username;
        string password;
        string userManager;
        string passManager;
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
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
    }
}
