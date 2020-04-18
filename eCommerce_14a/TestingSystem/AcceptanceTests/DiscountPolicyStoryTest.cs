using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-discount-policy-281 </req>
    [TestClass]
    public class DiscountPolicyStoryTest : SystemTrackTest
    {
        string guestID;
        string username;
        string password;
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
            guestID = enterSystem().Item1;
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
        //happy
        public void ValidDiscountPolicyTest() 
        {
            Assert.IsTrue(CheckDiscountPolicy(guestID, storeID).Item1, CheckDiscountPolicy(guestID, storeID).Item2);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(CheckDiscountPolicy("", storeID).Item1, CheckDiscountPolicy("", storeID).Item2);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPolicyTest()
        {
            Assert.IsFalse(CheckDiscountPolicy("       ", 10^6).Item1, CheckDiscountPolicy("       ", 10 ^ 6).Item2);
        }
    }
}
