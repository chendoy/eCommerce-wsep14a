using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-registration-22 </req>
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
            /// TODO: impl
        }

        [TestMethod]
        //happy
        public void LegalUserDetailsTest() 
        {
            string username = UserGenerator.RandomString(3);
            string password = UserGenerator.RandomString(5);
            Assert.IsTrue(Register(username, password).Item1, Register(username, password).Item2);
        }

        [TestMethod]
        //sad
        public void ShortUserNameTest()
        {
            string username = UserGenerator.RandomString(1);
            string password = UserGenerator.RandomString(5);
            Assert.IsFalse(Register(username, password).Item1, Register(username, password).Item2);
        }

        [TestMethod]
        //bad
        public void BlankUsernameAndPasswordRegisterTest()
        {
            Assert.IsFalse(Register("           ", "    ").Item1, Register("           ", "    ").Item2);
        }
    }
}
