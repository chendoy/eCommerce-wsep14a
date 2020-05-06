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
        string username = UserGenerator.GetValidUsernames()[0];
        string password = UserGenerator.GetPasswords()[0];
        int storeID;

        [TestInitialize]
        public void SetUp()
        {
            guestID = enterSystem().Item2;
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
            Assert.IsTrue(CheckDiscountPolicy(guestID, 1, true).Item1, CheckDiscountPolicy(guestID, 1, true).Item2);
        }

        [TestMethod]
        //sad
        public void InvalidUserIDPolicyTest()
        {
            Assert.IsFalse(CheckDiscountPolicy("", 0, false).Item1, CheckDiscountPolicy("", 0, false).Item2);
        }

        [TestMethod]
        //bad
        public void InvalidUserIDAndStoreIDPolicyTest()
        {
            Assert.IsFalse(CheckDiscountPolicy("       ", 0, false).Item1, CheckDiscountPolicy("       ", 0, false).Item2);
        }
    }
}
