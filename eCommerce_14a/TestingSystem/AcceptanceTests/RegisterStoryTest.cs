using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class RegisterStoryTest : SystemTrackTest
    {
        [TestInitialize]
        public void SetUp()
        {
            // TODO: impl
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }

        [TestMethod]
        //happy
        public void LegalUserDetailsTest() 
        {
            Assert.IsTrue(Register(UserGenerator.RandomString(3), UserGenerator.RandomString(5)));
        }
        [TestMethod]
        //sad
        public void ShortUserNameTest()
        {
            Assert.IsFalse(Register(UserGenerator.RandomString(1), UserGenerator.RandomString(5)));
        }
        [TestMethod]
        //bad
        public void UserLeftWhileRegisterTest()
        {
            GetDetailsFromUser();
            UserLeft();
            Assert.IsTrue(UserNotRegistered());
        }
    }
}
