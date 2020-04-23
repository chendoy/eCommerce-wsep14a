using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-login-23 </req>
    [TestClass]
    public class LoginStoryTest : SystemTrackTest
    {
        string[] validUsernames = UserGenerator.GetValidUsernames();
        string[] incorrectUsernames = UserGenerator.GetIncorrectUsernames(); // for future using
        string[] passwords = UserGenerator.GetPasswords();

        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void TearDown()
        {
            ClearAllUsers();
        }

        [TestMethod]
        //happy
        public void ExistingUsernameAndPasswordTest()
        {
            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE; i++)
            {
                Register(validUsernames[i], passwords[i]);
                Assert.IsTrue(Login(validUsernames[i], passwords[i]).Item1, Login(validUsernames[i], passwords[i]).Item2);
            }
        }

        [TestMethod]
        //sad
        public void UnmatchUsernameAndPasswordTest()
        {
            Register(validUsernames[1], passwords[1]);
            Register(validUsernames[2], passwords[2]);
            Assert.IsFalse(Login(validUsernames[1], passwords[2]).Item1, Login(validUsernames[1], passwords[2]).Item2);

            for (int i = 0; i < UserGenerator.FIXED_USERNAMES_SIZE; i++)
            {
                Register(validUsernames[i], passwords[i]);
                Assert.IsTrue(Login(validUsernames[i], passwords[i]).Item1, Login(validUsernames[i], passwords[i]).Item2);
            }
        }

        [TestMethod]
        //bad
        public void InvalidPasswordAndUsernameTest()
        {
            Assert.IsFalse(Login("@!#!#!@$%^$%^","  ").Item1, Login("@!#!#!@$%^$%^", "  ").Item2);
        }
    }
}
