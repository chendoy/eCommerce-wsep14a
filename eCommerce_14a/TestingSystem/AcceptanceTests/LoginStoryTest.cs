using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    [TestClass]
    public class LoginStoryTest : SystemTrackTest
    {
        String UserName1;
        String Password1;
        String UserName2;
        String Password2;
        [TestInitialize]
        public void SetUp()
        {
            String UserName1 = UserGenerator.RandomString(5);
            String Password1 = UserGenerator.RandomString(5);
            String UserName2 = UserGenerator.RandomString(5);
            String Password2 = UserGenerator.RandomString(5);
        }
        [TestCleanup]
        public void TearDown()
        {
            // TODO: impl
        }

        [TestMethod]
        //happy
        public void ExistingUsernameAndPasswordTest()
        {
            //pre-condition
            Register(UserName1, Password1);
            Assert.IsTrue(sys.Login(UserName1, Password1));
        }
        [TestMethod]
        //sad
        public void UnmatchUsernameAndPasswordTest()
        {
            Register(UserName1, Password1);
            Register(UserName2, Password2);
            Assert.IsFalse(Login(UserName1,UserName2));
        }
        [TestMethod]
        //bad
        public void UserLeftWhileLoginTest()
        {
            GetDetailsFromUser();
            UserLeft();
            Assert.IsTrue(UserNotLogin());
        }
    }
}
