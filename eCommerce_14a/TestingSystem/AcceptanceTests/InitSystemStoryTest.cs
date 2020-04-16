using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.AcceptanceTests
{
    /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-system-initialization-11 </req>
    [TestClass]
    public class InitSystemStoryTest : SystemTrackTest
    {
        string username;
        string password; 

        [TestCleanup]
        public void TearDown() 
        {
            // TODO: impl
        }

        [TestInitialize]
        public void SetUp() 
        {
            username = UserGenerator.RandomString(5);
            password = UserGenerator.RandomString(5);
        }

        [TestMethod]
        public void HealthySystemsTest()
        {
            Assert.IsTrue(Init().Item1, Init().Item2);
        }

        [TestMethod]
        //sad
        public void NoConnectionWithOneSystemTest()
        {
            setPaymentSystemConnection(false);
            Assert.IsFalse(Init().Item1, Init().Item2);
        }

        [TestMethod]
        //bad
        public void StartReqWhileBootingTest()
        {
            Register(username, password);
            Assert.IsFalse(Init().Item1, Init().Item2);
        }
    }
}
